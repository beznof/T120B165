namespace Languages.WebApi.Models.Requests.Auth;

public sealed class RefreshRequest
{
    /// <summary>
    /// The current refresh token.
    /// Required.
    /// Replaced after each successful refresh.
    /// </summary>
    /// <example>refresh token</example>
    public required string RefreshToken { get; set; }
}
