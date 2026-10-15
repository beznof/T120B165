using System.ComponentModel.DataAnnotations;
using Languages.Domain.Enums;

namespace Languages.WebApi.Models.Requests.Auth;

public sealed class RegisterRequest
{
    /// <summary>
    /// The username of the user.
    /// Leading and trailing whitespace is removed.
    /// Required.
    /// Must contain between 8 and 64 characters.
    /// </summary>
    /// <example>CT-5555</example>
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = null!;

    /// <summary>
    /// The password of the user.
    /// Leading and trailing whitespace is removed.
    /// Required.
    /// Must contain between 8 and 64 characters.
    /// </summary>
    /// <example>Coruscant123</example>
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = null!;

    /// <summary>
    /// The role of the user.
    /// 0 for Learner and 1 for Teacher, or alternatively use pure strings, such as: "Learner" and "Teacher" accordingly.
    /// Required.
    /// </summary>
    /// <example>Learner</example>
    [Required(ErrorMessage = "Role is required.")]
    public UserRole? Role { get; set; }
}