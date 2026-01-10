using CVA.Application.Contracts;
using CVA.Application.ProfileService;
using CVA.Domain.Interfaces;
using CVA.Domain.Models;
using CVA.Infrastructure.Auth;
using Microsoft.Extensions.Options;

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
    IRefreshTokenRepository refreshTokens,
    IGoogleTokenVerifier googleVerifier,
    IAppTokenIssuer tokenIssuer,
    IRefreshTokenProtector refreshTokenProtector,
    IOptions<RefreshTokenOptions> refreshTokenOptions,
    ICurrentUserAccessor currentUser) : IIdentityService
{
    private const string GoogleProviderName = "Google";

    private readonly RefreshTokenOptions _refreshOptions = refreshTokenOptions.Value
                                                           ?? throw new ArgumentNullException(nameof(refreshTokenOptions));

    /// <inheritdoc />
    public async Task<AuthTokenDto> SignInWithGoogleAsync(string googleIdToken, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(googleIdToken))
        {
            throw new ArgumentException("Google ID token is required.", nameof(googleIdToken));
        }

        var payload = await googleVerifier.VerifyAsync(googleIdToken, ct);
        var external = new ExternalIdentity(GoogleProviderName, payload.Subject, payload.Email);

        var user = await users.GetByGoogleSubjectAsync(external.Subject, ct);
        if (user is null)
        {
            user = User.CreateFromGoogle(external.Email, external.Subject, UserRole.User);
            await users.CreateAsync(user, ct);
        }

        var accessToken = tokenIssuer.Issue(user.Id, user.Role.ToString());
        var refreshToken = await IssueRefreshTokenAsync(user.Id, ct);

        return new AuthTokenDto(accessToken, refreshToken, user.Id);
    }

    /// <inheritdoc />
    public async Task<AuthTokenDto> RefreshAsync(string refreshToken, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ArgumentException("Refresh token is required.", nameof(refreshToken));
        }

        var tokenHash = refreshTokenProtector.Hash(refreshToken);
        var stored = await refreshTokens.GetByTokenHashAsync(tokenHash, ct);
        if (stored is null || !stored.IsActive)
        {
            throw new ApplicationException("Invalid or expired refresh token.");
        }

        var user = await users.GetByIdAsync(stored.UserId, ct);
        if (user is null)
        {
            throw new ApplicationException("User not found.");
        }

        var newRefreshToken = refreshTokenProtector.Generate();
        var newHash = refreshTokenProtector.Hash(newRefreshToken);
        var newExpiresAt = DateTimeOffset.UtcNow.AddDays(_refreshOptions.LifetimeDays);

        await refreshTokens.RevokeAsync(stored.Id, DateTimeOffset.UtcNow, newHash, ct);
        await refreshTokens.CreateAsync(RefreshToken.Create(user.Id, newHash, newExpiresAt), ct);

        var newAccessToken = tokenIssuer.Issue(user.Id, user.Role.ToString());
        return new AuthTokenDto(newAccessToken, newRefreshToken, user.Id);
    }

    /// <inheritdoc />
    public async Task<IdentityMe> GetMeAsync(CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated)
        {
            throw new ApplicationException("User is not authenticated.");
        }

        var user = await users.GetByIdAsync(currentUser.UserId, ct);
        return user is not null
            ? new IdentityMe(user.Id, user.Role.ToString(), user.Email.Value)
            : throw new ApplicationException("User not found.");
    }
    
    private async Task<string> IssueRefreshTokenAsync(Guid userId, CancellationToken ct)
    {
        var refreshToken = refreshTokenProtector.Generate();
        var tokenHash = refreshTokenProtector.Hash(refreshToken);
        var expiresAt = DateTimeOffset.UtcNow.AddDays(_refreshOptions.LifetimeDays);

        await refreshTokens.CreateAsync(RefreshToken.Create(userId, tokenHash, expiresAt), ct);
        return refreshToken;
    }
}