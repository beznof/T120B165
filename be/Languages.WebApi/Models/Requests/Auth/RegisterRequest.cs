using Languages.Domain.Enums;

namespace Languages.WebApi.Models.Requests.Auth;

public sealed class RegisterRequest
{
    /// <summary>
    /// The username of the user.
    /// Leading and trailing whitespace is removed.
    /// Required.
    /// Cannot exceed 50 characters after trimming.
    /// </summary>
    /// <example>CT-5555</example>
    public required string Username { get; set; }

    /// <summary>
    /// The password of the user.
    /// Required.
    /// Must contain between 8 and 64 characters.
    /// </summary>
    /// <example>Coruscant123</example>
    public required string Password { get; set; }

    /// <summary>
    /// The requested role. Learner is 0 and Teacher is 1.
    /// Required.
    /// Learners are approved automatically; teachers require approval.
    /// Admin registration is not allowed.
    /// </summary>
    /// <example>0</example>
    public required UserRole Role { get; set; }
}