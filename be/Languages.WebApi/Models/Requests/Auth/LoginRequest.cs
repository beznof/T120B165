namespace Languages.WebApi.Models.Requests.Auth;

public sealed class LoginRequest
{
    /// <summary>
    /// The username of the user.
    /// Required.
    /// </summary>
    /// <example>CT-5555</example>
    public required string Username { get; set; }

    /// <summary>
    /// The password of the user.
    /// Required.
    /// </summary>
    /// <example>Coruscant123</example>
    public required string Password { get; set; }
}