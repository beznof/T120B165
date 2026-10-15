using System.ComponentModel.DataAnnotations;

namespace Languages.WebApi.Models.Requests.Auth;

public sealed class LogoutRequest
{
    /// <summary>
    /// The current refresh token of the session to revoke.
    /// Required.
    /// </summary>
    /// <example>refresh token</example>
    [Required(ErrorMessage = "Refresh token is required.")]
    public string RefreshToken { get; set; } = null!;
}
