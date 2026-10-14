namespace Languages.WebApi.Models.Requests.Auth;

public sealed class RefreshRequest
{
    /// <summary>
    /// The refresh token.
    /// Required.
    /// </summary>
    /// <example>refresh token</example>
    public required string RefreshToken { get; set; }
}
