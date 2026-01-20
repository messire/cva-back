using CVA.Application.IdentityService;
using CVA.Presentation.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace CVA.Presentation.Web;

/// <summary>
/// Public authentication.
/// </summary>
[ApiController]
[Route("api/auth")]
public sealed class AuthController(IIdentityService identityService, IGoogleOAuthFlow googleOAuthFlow, IOneTimeCodeStore oneTimeCodeStore)
    : ControllerBase
{
    /// <summary>
    /// Authenticates the user using a Google ID token and returns an application JWT.
    /// If the user does not exist, it will be created.
    /// </summary>
    /// <param name="request">Request containing Google ID token.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPost("google")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthTokenDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthTokenDto>> GoogleSignIn([FromBody] GoogleSignInRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.IdToken))
        {
            return BadRequest(new { message = "Google idToken is required." });
        }

        var token = await identityService.SignInWithGoogleAsync(request.IdToken, ct);
        return Ok(token);
    }

    /// <summary>
    /// Exchanges a valid refresh token for a new access token and a new refresh token.
    /// Refresh tokens are rotated (one-time use).
    /// </summary>
    /// <param name="request">Refresh token request.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthTokenDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthTokenDto>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Problem(
                title: "Invalid request",
                detail: "refreshToken is required.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var token = await identityService.RefreshAsync(request.RefreshToken, ct);
        return Ok(token);
    }

    /// <summary>
    /// Returns minimal identity info for the current authenticated user.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet("whoami")]
    [Authorize]
    [ProducesResponseType(typeof(IdentityMe), StatusCodes.Status200OK)]
    public async Task<ActionResult<IdentityMe>> Me(CancellationToken ct)
    {
        var me = await identityService.GetMeAsync(ct);
        return Ok(me);
    }

    /// <summary>
    /// Starts backend-driven Google OAuth redirect flow.
    /// </summary>
    /// <param name="returnUrl">Absolute frontend return URL.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet("google/start")]
    [AllowAnonymous]
    public Task<IActionResult> GoogleStart([FromQuery] string? returnUrl, CancellationToken ct)
        => Task.FromResult(googleOAuthFlow.Start(returnUrl));

    /// <summary>
    /// Handles Google OAuth callback.
    /// Exchanges Google auth code for a Google ID token, signs in user, and redirects to frontend with a one-time code.
    /// </summary>
    /// <param name="code">Google authorization code.</param>
    /// <param name="state">OAuth state.</param>
    /// <param name="error">OAuth error code.</param>
    /// <param name="errorDescription">OAuth error description.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet("google/callback")]
    [AllowAnonymous]
    public Task<IActionResult> GoogleCallback([FromQuery] string? code, [FromQuery] string? state, [FromQuery] string? error, [FromQuery] string? errorDescription, CancellationToken ct)
        => googleOAuthFlow.Callback(code, state, error, errorDescription, ct);

    /// <summary>
    /// Exchanges a one-time code for application JWT tokens.
    /// This is the only endpoint that needs CORS for SPA fetch.
    /// </summary>
    /// <param name="request">One-time code exchange request.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPost("exchange")]
    [AllowAnonymous]
    [EnableCors("AuthExchange")]
    [ProducesResponseType(typeof(AuthTokenDto), StatusCodes.Status200OK)]
    public Task<ActionResult<AuthTokenDto>> Exchange([FromBody] AuthCodeExchangeRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return Task.FromResult<ActionResult<AuthTokenDto>>(Problem(
                title: "Invalid request",
                detail: "code is required.",
                statusCode: StatusCodes.Status400BadRequest));
        }

        if (!oneTimeCodeStore.TryConsume(request.Code, out var token))
        {
            return Task.FromResult<ActionResult<AuthTokenDto>>(Problem(
                title: "Invalid or expired code",
                detail: "The one-time code is invalid or has expired.",
                statusCode: StatusCodes.Status400BadRequest));
        }

        return Task.FromResult<ActionResult<AuthTokenDto>>(Ok(token));
    }
}