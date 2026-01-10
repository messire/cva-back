using System.Net;
using System.Net.Http.Json;
using CVA.Infrastructure.Auth;
using Microsoft.Extensions.Options;

namespace CVA.Presentation.Auth;

/// <summary>
/// Google OAuth client implementation.
/// </summary>
/// <param name="httpClient">HTTP client.</param>
/// <param name="options">Google authentication options.</param>
internal sealed class GoogleOAuthClient(HttpClient httpClient, IOptions<GoogleAuthOptions> options) : IGoogleOAuthClient
{
    /// <inheritdoc />
    public async Task<string> ExchangeCodeForIdTokenAsync(string authorizationCode, string redirectUri, CancellationToken ct)
    {
        ValidateAuthOptions(authorizationCode);

        var form = new Dictionary<string, string>
        {
            ["code"] = authorizationCode,
            ["client_id"] = options.Value.ClientId,
            ["client_secret"] = options.Value.ClientSecret,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code"
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token");
        request.Content = new FormUrlEncodedContent(form);

        using var response = await httpClient.SendAsync(request, ct);
        var payload = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: ct);

        ValidatePayload(payload, response.IsSuccessStatusCode, response.StatusCode);
        return payload?.IdToken!;
    }

    private void ValidateAuthOptions(string authorizationCode)
    {
        if (string.IsNullOrWhiteSpace(authorizationCode))
        {
            throw new ArgumentException("Authorization code is required.", nameof(authorizationCode));
        }

        if (string.IsNullOrWhiteSpace(options.Value.ClientId))
        {
            throw new InvalidOperationException("Google ClientId is not configured.");
        }

        if (string.IsNullOrWhiteSpace(options.Value.ClientSecret))
        {
            throw new InvalidOperationException("Google ClientSecret is not configured.");
        }
    }

    private static void ValidatePayload(TokenResponse? payload, bool isSuccess, HttpStatusCode statusCode)
    {
        if (!isSuccess || payload is null)
        {
            throw new InvalidOperationException($"Google OAuth token exchange failed ({(int)statusCode}).");
        }

        if (!string.IsNullOrWhiteSpace(payload.Error))
        {
            throw new InvalidOperationException($"Google OAuth error: {payload.Error} ({payload.ErrorDescription}).");
        }

        if (string.IsNullOrWhiteSpace(payload.IdToken))
        {
            throw new InvalidOperationException("Google OAuth response did not contain id_token.");
        }
    }
}