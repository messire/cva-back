using System.Security.Claims;
using CVA.Presentation.Web.Auth.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace CVA.Presentation.Web;

/// <summary>
/// Debug JWT endpoints: health-check, viewing current hallmarks, and issuing a dev-token.
/// dev-token should only be called in the Development environment.
/// </summary>
[ApiController]
[Route("api/_auth")]
public sealed class AuthDebugController(JwtTokenGenerator tokenGenerator, IHostEnvironment environment) : ControllerBase
{
    /// <summary>
    /// Health check. Always 200.
    /// </summary>
    [HttpGet("ping")]
    [AllowAnonymous]
    public IActionResult Ping()
    {
        return Ok(new { status = "ok" });
    }

    /// <summary>
    /// Shows the current user and their claims.
    /// Requires a valid JWT.
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var claims = User.Claims
            .Select(c => new { c.Type, c.Value })
            .ToArray();

        return Ok(new
        {
            isAuthenticated = User.Identity?.IsAuthenticated,
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub"),
            role = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role"),
            claims
        });
    }

    /// <summary>
    /// DEV-ONLY. Issues a JWT based on the passed userId and role.
    /// </summary>
    [HttpPost("dev-token")]
    [AllowAnonymous]
    public IActionResult DevToken([FromBody] DevTokenRequest request)
    {
        if (!environment.IsDevelopment())
        {
            return NotFound();
        }

        var token = tokenGenerator.Generate(request.UserId, request.Role ?? "User");
        return Ok(new
        {
            accessToken = token
        });
    }

    /// <summary>
    /// Request model for issuing a dev token.
    /// </summary>
    public sealed class DevTokenRequest
    {
        /// <summary>
        /// The user ID that will be included in the claim <c>sub</c>.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// User role; defaults to "User".
        /// </summary>
        public string? Role { get; init; }
    }
}
