using System.ComponentModel.DataAnnotations;

namespace Languages.WebApi.Models.Requests.Auth;

public sealed class LoginRequest
{
    /// <summary>
    /// The username of the user.
    /// Required.
    /// </summary>
    /// <example>CT-5555</example>
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = null!;

    /// <summary>
    /// The password of the user.
    /// Required.
    /// </summary>
    /// <example>Coruscant123</example>
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = null!;
}