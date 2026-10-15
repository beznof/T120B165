using System.ComponentModel.DataAnnotations;

namespace Languages.WebApi.Models.Requests.Auth;

public sealed class RefreshRequest
{
    /// <summary>
    /// The refresh token.
    /// Required.
    /// </summary>
    /// <example>refresh token</example>
    [Required(ErrorMessage = "Refresh token is required.")]
    public string RefreshToken { get; set; } = null!;
}
