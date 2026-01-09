using Google.Apis.Auth;

namespace CVA.Infrastructure.Auth;

/// <summary>
/// Verifies Google ID tokens.
/// </summary>
/// <param name="options">Google auth options.</param>
internal sealed class GoogleIdTokenVerifier(GoogleAuthOptions options)
{
    private readonly GoogleAuthOptions _options = options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>
    /// Validates a Google ID token and returns its payload.
    /// </summary>
    /// <param name="idToken">Google ID token.</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task<GoogleJsonWebSignature.Payload> VerifyAsync(string idToken, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(idToken))
        {
            throw new ArgumentException("ID token is empty.", nameof(idToken));
        }

        if (string.IsNullOrWhiteSpace(_options.ClientId))
        {
            throw new InvalidOperationException("Auth:Google:ClientId is not configured.");
        }

        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [_options.ClientId]
        };

        return await GoogleJsonWebSignature.ValidateAsync(idToken, settings).WaitAsync(ct);
    }
}