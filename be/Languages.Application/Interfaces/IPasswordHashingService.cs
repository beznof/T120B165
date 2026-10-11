namespace Languages.Application.Interfaces;

public interface IPasswordHashingService
{
    string HashPassword(string password);
    bool VerifyPasswordHash(string passwordHash, string password);
}
