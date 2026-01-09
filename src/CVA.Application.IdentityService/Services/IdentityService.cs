using CVA.Application.Contracts;
using CVA.Application.ProfileService;
using CVA.Domain.Interfaces;
using CVA.Domain.Models;
using CVA.Infrastructure.Auth;

namespace CVA.Application.IdentityService;

/// <summary>
/// Default implementation of <see cref="IIdentityService"/>.
/// Handles external sign-in (Google) and the retrieval of the current user's identity info.
/// </summary>
/// <param name="users">User repository.</param>
/// <param name="googleVerifier">Google ID token verifier.</param>
/// <param name="tokenIssuer">Application JWT issuer.</param>
/// <param name="currentUser">Accessor for the current user identity (claims-based).</param>
internal sealed class IdentityService(
    IUserRepository users,
    IGoogleTokenVerifier googleVerifier,
    IAppTokenIssuer tokenIssuer,
    ICurrentUserAccessor currentUser) : IIdentityService
{
    private const string GoogleProviderName = "Google";
    
    /// <inheritdoc />
    public async Task<AuthTokenDto> SignInWithGoogleAsync(string googleIdToken, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(googleIdToken))
        {
            throw new ArgumentException("Google ID token is required.", nameof(googleIdToken));
        }

        var payload = await googleVerifier.VerifyAsync(googleIdToken, ct);
        if (payload is null)
        {
            throw new ApplicationException("Invalid Google ID token.");
        }

        var external = new ExternalIdentity(GoogleProviderName, payload.Subject, payload.Email);
        var user = await users.GetByGoogleSubjectAsync(external.Subject, ct);

        if (user is null)
        {
            user = User.CreateFromGoogle(external.Email, external.Subject, UserRole.User);
            await users.CreateAsync(user, ct);
        }

        var token = tokenIssuer.Issue(user.Id, user.Role.ToString());

        return new AuthTokenDto(token, user.Id);
    }

    /// <inheritdoc />
    public async Task<IdentityMe> GetMeAsync(CancellationToken ct)
    {
        var currentUserId = currentUser.UserId;
        var user = await users.GetByIdAsync(currentUserId, ct);
        return user is not null
            ? new IdentityMe(user.Id, user.Role.ToString(), user.Email.Value)
            : throw new ApplicationException("User not found.");
    }
}