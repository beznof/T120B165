namespace Languages.WebApi.Models.Requests.Auth;

public sealed class LogoutRequest
{
    /// <summary>
    /// The current refresh token of the session to revoke.
    /// Required.
    /// </summary>
    /// <example>refresh token</example>
    public required string RefreshToken { get; set; }
}
