namespace Languages.Infrastructure.Options;

public sealed class AuthOptions
{
    public const string AppSettingsSectionName = "Auth";

    public required string JwtIssuer { get; set; }
    public required string JwtAudience { get; set; }
    public required string JwtSigningKey { get; set; }
    public required int JwtAccessTokenLifetimeMinutes { get; set; }
    public required int JwtRefreshTokenLifetimeDays { get; set; }
}
