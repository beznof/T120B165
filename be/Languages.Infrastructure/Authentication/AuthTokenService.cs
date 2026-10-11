using System.Security.Cryptography;
using Languages.Application.Interfaces;
using Languages.Domain.Entities;
using Languages.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Languages.Infrastructure.Authentication;

public sealed class AuthTokenService(
    IOptions<AuthOptions> authOptions) : IAuthTokenService
{
    private readonly AuthOptions _authOptions = authOptions.Value;

    public LanguagesAccessToken CreateAccessToken(User user)
    {
        var now = DateTime.UtcNow;
        var expiresAtUTC = now.AddMinutes(_authOptions.JwtAccessTokenLifetimeMinutes);

        var key = new SymmetricSecurityKey(Convert.FromBase64String(_authOptions.JwtSigningKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _authOptions.JwtIssuer,
            Audience = _authOptions.JwtAudience,
            IssuedAt = now,
            NotBefore = now,
            Expires = expiresAtUTC,
            SigningCredentials = signingCredentials,
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),
                [JwtRegisteredClaimNames.Name] = user.Username,
                ["role"] = user.Role.ToString(),
                [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString("D")
            }
        };

        return new LanguagesAccessToken(
            Value: new JsonWebTokenHandler().CreateToken(descriptor), 
            ExpiresAtUTC: expiresAtUTC
        );
    }

    public LanguagesRefreshToken CreateRefreshToken(DateTime? originalExpiresAtUTC = null)
    {
        var now = DateTime.UtcNow;
        var expiresAtUTC = originalExpiresAtUTC ?? now.AddDays(_authOptions.JwtRefreshTokenLifetimeDays);

        var randomBytes = RandomNumberGenerator.GetBytes(128);
        var value = Base64UrlEncoder.Encode(randomBytes);
        var hashedValue = Base64UrlEncoder.Encode(SHA256.HashData(randomBytes));

        return new LanguagesRefreshToken(
            Value: value,
            Hash: hashedValue,
            ExpiresAtUTC: expiresAtUTC
        );
    }

    public string HashRefreshToken(string rawRefreshToken)
    {
        var rawRefreshTokenBytes = Base64UrlEncoder.DecodeBytes(rawRefreshToken);
        var hashedRefreshToken = Base64UrlEncoder.Encode(SHA256.HashData(rawRefreshTokenBytes));
        
        return hashedRefreshToken;
    }
}
