namespace Languages.WebApi.Models.Requests.Auth;

public sealed class LoginRequest
{
    /// <summary>
    /// The username of the user.
    /// Required.
    /// Leading and trailing whitespace is removed.</summary>
    /// <example>CT-5555</example>
    public required string Username { get; set; }

    /// <summary>
    /// The password of the user.
    /// Required.
    /// Cannot exceed 128 characters.
    /// </summary>
    /// <example>Coruscant123</example>
    public required string Password { get; set; }
}