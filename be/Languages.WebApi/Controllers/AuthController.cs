using System.Net;
using Languages.Application.Interfaces;
using Languages.Application.Models.Inputs.Auth;
using Languages.Application.Models.Outputs;
using Languages.WebApi.Extensions;
using Languages.WebApi.Filters;
using Languages.WebApi.Models.Common;
using Languages.WebApi.Models.Requests.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Controllers;

/// <summary>User authentication.</summary>
[ApiController]
[AllowAnonymous]
[Route("api/auth")]
[Produces("application/json")]
[NoStore]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Register a user.</summary>
    /// <remarks>Register a user by specifying a username, password, and Learner or Teacher role.</remarks>
    /// <response code="201">The user was registered successfully.</response>
    /// <response code="400">The request JSON body was malformed or field validation failed.</response>
    /// <response code="409">The username was already in use.</response>
    [HttpPost("register")]
    [ProducesResponseType<ApiResponse<RegisterOutput>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<RegisterOutput>>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var username = request.Username;
        var password = request.Password;
        var role = request.Role!.Value;

        var result = await authService.Register(new RegisterInput
        {
            Username = username,
            Password = password,
            Role = role
        }, cancellationToken);

        return result.ToActionResult(statusCode: result.IsSuccessful ? HttpStatusCode.Created : null);
    }

    /// <summary>Log in a user.</summary>
    /// <remarks>Log in using a username and password. The access and refresh tokens are returned in the JSON body.</remarks>
    /// <response code="200">The user was logged in successfully.</response>
    /// <response code="400">The JSON body was malformed or field validation failed.</response>
    /// <response code="401">The username or password is incorrect.</response>
    [HttpPost("login")]
    [ProducesResponseType<ApiResponse<AuthTokensOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthTokensOutput>>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var username = request.Username;
        var password = request.Password;

        var result = await authService.Login(new LoginInput
        {
            Username = username,
            Password = password
        }, cancellationToken);

        return result.ToActionResult();
    }

    /// <summary>Refresh authentication tokens.</summary>
    /// <remarks>Supply the current refresh token in the JSON body. The access and replacement refresh tokens are returned in the JSON body. The session's expiration remains unchanged.</remarks>
    /// <response code="200">The tokens were refreshed successfully.</response>
    /// <response code="400">The JSON body was malformed or field validation failed.</response>
    /// <response code="401">The refresh token is invalid or has expired.</response>
    [HttpPost("refresh")]
    [ProducesResponseType<ApiResponse<AuthTokensOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthTokensOutput>>> Refresh([FromBody] RefreshRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = request.RefreshToken;

        var result = await authService.RefreshAccessToken(new RefreshInput
        {
            RefreshToken = refreshToken
        }, cancellationToken);

        return result.ToActionResult();
    }

    /// <summary>Log out a user session.</summary>
    /// <remarks>Supply the current refresh token in the JSON body to revoke the session. Already-issued access tokens remain valid until they expire.</remarks>
    /// <response code="200">The user was logged out successfully.</response>
    /// <response code="400">The JSON body was malformed or incorrect.</response>
    /// <response code="401">The refresh token is invalid or has already been revoked.</response>
    [HttpPost("logout")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse>> Logout([FromBody] LogoutRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = request.RefreshToken;

        var result = await authService.Logout(new LogoutInput
        {
            RefreshToken = refreshToken
        }, cancellationToken);

        return result.ToActionResult();
    }
}
