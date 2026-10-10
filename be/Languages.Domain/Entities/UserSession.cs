namespace Languages.Domain.Entities;

public sealed class UserSession : BaseEntity
{
    public required string RefreshTokenHash { get; set; }
    public required DateTime ExpiresAtUTC { get; set; }
    public DateTime? RevokedAtUTC { get; set; }

    public int UserId { get; init; }

    public required User User { get; init; }
}
