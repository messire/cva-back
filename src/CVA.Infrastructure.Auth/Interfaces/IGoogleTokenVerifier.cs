using Google.Apis.Auth;

namespace CVA.Infrastructure.Auth;

/// <summary>
/// Google token verifier for authenticated users.
/// </summary>
public interface IGoogleTokenVerifier
{
    /// <summary>
    /// Validates a Google ID token and returns its payload.
    /// </summary>
    /// <param name="idToken">Google ID token.</param>
    /// <param name="ct">Cancellation token.</param>
    public Task<GoogleJsonWebSignature.Payload> VerifyAsync(string idToken, CancellationToken ct);
}