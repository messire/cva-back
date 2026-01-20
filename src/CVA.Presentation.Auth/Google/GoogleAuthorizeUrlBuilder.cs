using System.Text;

namespace CVA.Presentation.Auth;

/// <summary>
/// Responsible for building Google OAuth authorization URLs.
/// </summary>
/// <param name="clientId">Google OAuth client identifier.</param>
internal sealed class GoogleAuthorizeUrlBuilder(string clientId)
{
    /// <summary>
    /// Builds a Google OAuth authorization URL.
    /// </summary>
    /// <param name="redirectUri">OAuth redirect URI registered in Google Console.</param>
    /// <param name="state">OAuth state value.</param>
    /// <returns>Authorization URL.</returns>
    public string Build(string redirectUri, string state)
    {
        const string scope = "openid email profile";

        var sb = new StringBuilder("https://accounts.google.com/o/oauth2/v2/auth");
        sb.Append("?response_type=code");
        sb.Append("&client_id=").Append(Uri.EscapeDataString(clientId));
        sb.Append("&redirect_uri=").Append(Uri.EscapeDataString(redirectUri));
        sb.Append("&scope=").Append(Uri.EscapeDataString(scope));
        sb.Append("&state=").Append(Uri.EscapeDataString(state));
        sb.Append("&prompt=select_account");

        return sb.ToString();
    }
}