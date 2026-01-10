namespace CVA.Presentation.Auth;

/// <summary>
/// Performs OAuth authorization code exchange with Google.
/// </summary>
public interface IGoogleOAuthClient
{
    /// <summary>
    /// Exchanges a Google authorization code for an ID token.
    /// </summary>
    /// <param name="authorizationCode">Authorization code received from Google OAuth callback.</param>
    /// <param name="redirectUri">Redirect URI that was used during authorization.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Google ID token.</returns>
    Task<string> ExchangeCodeForIdTokenAsync(string authorizationCode, string redirectUri, CancellationToken ct);
}