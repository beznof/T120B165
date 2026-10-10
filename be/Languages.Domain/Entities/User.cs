using Languages.Domain.Enums;

namespace Languages.Domain.Entities;

public sealed class User : BaseEntity
{
    public required string Username { get; init; }
    public required string PasswordHash { get; init; }
    public UserRole Role { get; init; } = UserRole.Learner;
    public required bool IsAccountApproved { get; set; }

    public ICollection<UserSession> UserSessions { get; set; } = [];
}
