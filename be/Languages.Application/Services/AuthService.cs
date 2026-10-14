using Languages.Application.Extensions;
using Languages.Application.Interfaces;
using Languages.Application.Models.Common;
using Languages.Application.Models.Inputs.Auth;
using Languages.Application.Models.Outputs;
using Languages.Application.Persistence;
using Languages.Domain.Entities;
using Languages.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Languages.Application.Services;

public sealed class AuthService(
    ILanguagesDbContext dbContext,
    IInputValidation validation,
    IPasswordHashingService passwordHashingService,
    IAuthTokenService tokenService) : IAuthService
{
    public async Task<Result<RegisterOutput>> Register(RegisterInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<RegisterOutput>();

        var username = input.Username.Trim();
        if (await dbContext.Users.AnyAsync(u => u.Username == username, cancellationToken))
            return Result.Failure<RegisterOutput>("Username is already in use.", ResultErrorKind.ConflictOccured);

        var user = new User
        {
            Username = username,
            PasswordHash = passwordHashingService.HashPassword(input.Password.Trim()),
            Role = input.Role
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success("User was registered successfully.", new RegisterOutput(
            Id: user.Id,
            Username: user.Username,
            Role: user.Role,
            CreatedAtUTC: user.CreatedAtUTC
        ));
    }

    public async Task<Result<AuthTokensOutput>> Login(LoginInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<AuthTokensOutput>();

        var username = input.Username.Trim();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        
        if (user == null || !passwordHashingService.VerifyPasswordHash(user.PasswordHash, input.Password.Trim()))
            return Result.Failure<AuthTokensOutput>("Username or password is incorrect.", ResultErrorKind.AuthenticationFailed);

        var refreshToken = tokenService.CreateRefreshToken();
        
        var session = new UserSession
        {
            UserId = user.Id,
            User = user,
            RefreshTokenHash = refreshToken.Hash,
            ExpiresAtUTC = refreshToken.ExpiresAtUTC
        };
        
        dbContext.UserSessions.Add(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        var accessToken = tokenService.CreateAccessToken(user);
        
        return Result.Success("Logged in successfully.", new AuthTokensOutput(
            AccessToken: accessToken.Value,
            AccessTokenExpiresAtUTC: accessToken.ExpiresAtUTC,
            RefreshToken: refreshToken.Value,
            RefreshTokenExpiresAtUTC: refreshToken.ExpiresAtUTC
        ));
    }

    public async Task<Result<AuthTokensOutput>> RefreshAccessToken(RefreshInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<AuthTokensOutput>();

        var now = DateTime.UtcNow;
        
        string refreshTokenHash;
        try
        {
            refreshTokenHash = tokenService.HashRefreshToken(input.RefreshToken);
        }
        catch (FormatException)
        {
            return Result.Failure<AuthTokensOutput>("Refresh token is invalid.", ResultErrorKind.AuthenticationFailed);
        }
        
        var session = await dbContext.UserSessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => 
                    s.RefreshTokenHash == refreshTokenHash && s.RevokedAtUTC == null && s.ExpiresAtUTC > now, 
                cancellationToken);

        if (session == null)
            return Result.Failure<AuthTokensOutput>("Refresh token is invalid or has expired.", ResultErrorKind.AuthenticationFailed);

        var refreshToken = tokenService.CreateRefreshToken(session.ExpiresAtUTC);
        var accessToken = tokenService.CreateAccessToken(session.User);
        
        session.RefreshTokenHash = refreshToken.Hash;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success<AuthTokensOutput>("Tokens were refreshed successfully.", new AuthTokensOutput(
            AccessToken: accessToken.Value,
            AccessTokenExpiresAtUTC: accessToken.ExpiresAtUTC,
            RefreshToken: refreshToken.Value,
            RefreshTokenExpiresAtUTC: refreshToken.ExpiresAtUTC
        ));
    }

    public async Task<Result> Logout(LogoutInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult();
        
        string refreshTokenHash;
        try
        {
            refreshTokenHash = tokenService.HashRefreshToken(input.RefreshToken);
        }
        catch (FormatException)
        {
            return Result.Failure("Refresh token is invalid.", ResultErrorKind.AuthenticationFailed);
        }
        
        var session = await dbContext.UserSessions
            .Where(s => s.RefreshTokenHash == refreshTokenHash && s.RevokedAtUTC == null)
            .FirstOrDefaultAsync(cancellationToken);

        if (session == null)
            return Result.Failure("Refresh token is invalid or has already expired.", ResultErrorKind.AuthenticationFailed);
        
        session.RevokedAtUTC = DateTime.UtcNow;
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success("User was logged out successfully.");
    }
}
