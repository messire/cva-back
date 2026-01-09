using CVA.Application.IdentityService;
using Microsoft.AspNetCore.Authorization;

namespace CVA.Presentation.Web;

/// <summary>
/// Public authentication.
/// </summary>
[ApiController]
[Route("api/auth")]
public sealed class AuthController(IIdentityService identityService) : ControllerBase
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
}