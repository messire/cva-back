using CVA.Application.Contracts;

namespace CVA.Application.IdentityService;


/// <summary>
/// Provides operations related to authenticated user identity:
/// external sign-in and retrieval of the current user's identity information.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Authenticates the user using a Google ID token and issues an application JWT.
    /// If the user does not exist yet, it will be created.
    /// </summary>
    /// <param name="googleIdToken">Google ID token received from the client.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Application authentication token information.</returns>
    Task<AuthTokenDto> SignInWithGoogleAsync(string googleIdToken, CancellationToken ct);

    /// <summary>
    /// Returns minimal identity information for the currently authenticated user.
    /// This is NOT a public developer profile.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Minimal identity information about the current user.</returns>
    Task<IdentityMe> GetMeAsync(CancellationToken ct);
}