using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace CVA.Infrastructure.Auth;

/// <summary>
/// Verifies Google ID tokens.
/// </summary>
/// <param name="options">Google auth options.</param>
internal sealed class GoogleIdTokenVerifier(IOptions<GoogleAuthOptions> options) : IGoogleTokenVerifier
{
    private readonly GoogleAuthOptions _options = options.Value ?? throw new ArgumentNullException(nameof(options));

    /// <iheritdoc/>
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