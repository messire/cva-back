namespace CVA.Infrastructure.Auth;

/// <summary>
/// Google authentication options.
/// </summary>
public sealed class GoogleAuthOptions
{
    /// <summary>
    /// Configuration section path.
    /// </summary>
    public const string Path = "Auth:Google";

    /// <summary>
    /// Google OAuth client id (used to validate ID tokens).
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Google OAuth client secret (required for authorization code exchange).
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;
}