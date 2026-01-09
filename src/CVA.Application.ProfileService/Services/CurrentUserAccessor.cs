using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CVA.Application.ProfileService;


/// <summary>
/// ASP.NET Core implementation of <see cref="ICurrentUserAccessor"/>.
/// </summary>
internal sealed class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    /// <inheritdoc />
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

    /// <inheritdoc />
    public Guid UserId
    {
        get
        {
            var context = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available.");

            var user = context.User;
            if (user.Identity?.IsAuthenticated != true)
            {
                throw new InvalidOperationException("User is not authenticated.");
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("sub");
            if (userIdClaim is null)
            {
                throw new InvalidOperationException("User identifier claim not found.");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new InvalidOperationException("User identifier claim is not a valid GUID.");
            }

            return userId;
        }
    }
}