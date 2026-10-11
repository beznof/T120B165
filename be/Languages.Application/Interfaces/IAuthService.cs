using Languages.Application.Models.Common;
using Languages.Application.Models.Inputs.Auth;
using Languages.Application.Models.Outputs;

namespace Languages.Application.Interfaces;

public interface IAuthService
{
    Task<Result<RegisterOutput>> Register(RegisterInput input, CancellationToken cancellationToken);
    Task<Result<AuthTokensOutput>> Login(LoginInput input, CancellationToken cancellationToken);
    Task<Result<AuthTokensOutput>> RefreshAccessToken(RefreshInput input, CancellationToken cancellationToken);
    Task<Result> Logout(LogoutInput input, CancellationToken cancellationToken);
}