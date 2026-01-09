using CVA.Application.Abstractions;
using CVA.Application.Contracts;

namespace CVA.Application.AuthService;

/// <summary>
/// Provides authentication services (external login → app token).
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Signs in using Google ID token and returns an application access token.
    /// </summary>
    /// <param name="googleIdToken">Google ID token.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<Result<AuthTokenDto>> SignInWithGoogleAsync(string googleIdToken, CancellationToken ct);
}

