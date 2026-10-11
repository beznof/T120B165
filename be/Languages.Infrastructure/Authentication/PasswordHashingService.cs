using Languages.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Languages.Infrastructure.Authentication;

public sealed class PasswordHashingService (
    PasswordHasher<object> hasher
    ): IPasswordHashingService
{
    private readonly object user = new();
    public string HashPassword(string password) => hasher.HashPassword(user, password);

    public bool VerifyPasswordHash(string passwordHash, string password)
    {
        var result = hasher.VerifyHashedPassword(user, passwordHash, password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
