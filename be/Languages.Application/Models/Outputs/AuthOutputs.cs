using Languages.Domain.Enums;

namespace Languages.Application.Models.Outputs;

/// <summary>The registered user.</summary>
/// <param name="Id">The user's identifier.</param>
/// <param name="Username">The username of the user.</param>
/// <param name="Role">The role of the user. Learner is 0 and Teacher is 1.</param>
/// <param name="CreatedAtUTC">The date and time when the user was created, in UTC.</param>
/// <example>
/// {
///   "id": 1,
///   "username": "Captain Rex",
///   "role": 0,
///   "createdAtUTC": "2026-10-11T10:00:00Z"
/// }
/// </example>
public sealed record RegisterOutput(
    int Id,
    string Username,
    UserRole Role,
    DateTime CreatedAtUTC
);

/// <summary>The issued access and refresh tokens.</summary>
/// <param name="AccessToken">The access token.</param>
/// <param name="RefreshToken">The refresh token.</param>
/// <param name="AccessTokenExpiresAtUTC">The date and time when the access token expires, in UTC.</param>
/// <param name="RefreshTokenExpiresAtUTC">The date and time when the refresh token expires, in UTC.</param>
/// <example>
/// {
///   "accessToken": "hhhhhh.pppppp.ssssss",
///   "accessTokenExpiresAtUTC": "2026-10-11T10:15:00Z",
/// }
/// </example>

public sealed record AuthTokensOutput(
    string AccessToken,
    DateTime AccessTokenExpiresAtUTC,
    string RefreshToken,
    DateTime RefreshTokenExpiresAtUTC
);
