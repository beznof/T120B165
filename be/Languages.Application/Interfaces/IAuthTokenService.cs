using Languages.Domain.Entities;

namespace Languages.Application.Interfaces;

public interface IAuthTokenService
{
    LanguagesAccessToken CreateAccessToken(User user);
    LanguagesRefreshToken CreateRefreshToken(DateTime? expiresAtUTC = null);
    string HashRefreshToken(string rawRefreshToken);
}

public sealed record LanguagesAccessToken(string Value, DateTime ExpiresAtUTC);
public sealed record LanguagesRefreshToken(string Value, string Hash, DateTime ExpiresAtUTC);
