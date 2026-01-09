namespace CVA.Infrastructure.Auth;


/// <summary>
/// Represents configuration options for JWT authentication.
/// </summary>
public sealed class JwtOptions
{
    /// <summary>
    /// The configuration section path for JWT options.
    /// </summary>
    public const string Path = "Jwt";

    /// <summary>
    /// Token issuer.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Token audience.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Symmetric signing key used to sign JWT tokens.
    /// Must be a sufficiently long random secret.
    /// </summary>
    public string SigningKey { get; set; } = string.Empty;

    /// <summary>
    /// Access token lifetime in minutes.
    /// </summary>
    public int LifetimeMinutes { get; set; } = 60;
}