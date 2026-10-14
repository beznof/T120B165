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
    public required string Username { get; set; }

    /// <summary>
    /// The password of the user.
    /// Leading and trailing whitespace is removed.
    /// Required.
    /// Must contain between 8 and 64 characters.
    /// </summary>
    /// <example>Coruscant123</example>
    public required string Password { get; set; }

    /// <summary>
    /// The role of the user.
    /// 0 for Learner and 1 for Teacher, or alternatively use pure strings, such as: "Learner" and "Teacher" accordingly.
    /// Required.
    /// </summary>
    /// <example>Learner</example>
    public required UserRole Role { get; set; }
}