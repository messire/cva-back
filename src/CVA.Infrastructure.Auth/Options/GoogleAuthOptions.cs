namespace CVA.Infrastructure.Auth;

/// <summary>
/// Google authentication options.
/// </summary>
internal sealed class GoogleAuthOptions
{
    /// <summary>
    /// Configuration section path.
    /// </summary>
    public const string Path = "Auth:Google";

    /// <summary>
    /// Google OAuth client id (used to validate ID tokens).
    /// </summary>
    public string ClientId { get; set; } = string.Empty;
}