namespace CVA.Infrastructure.Auth;

/// <summary>
/// Configuration options for refresh tokens.
/// </summary>
public sealed class RefreshTokenOptions
{
    /// <summary>
    /// The configuration section path for refresh token options.
    /// </summary>
    public const string Path = "RefreshTokens";

    /// <summary>
    /// Refresh token lifetime in days.
    /// </summary>
    public int LifetimeDays { get; set; } = 30;

    /// <summary>
    /// Pepper secret used for hashing refresh tokens.
    /// This value must be configured in production.
    /// </summary>
    public string Pepper { get; set; } = string.Empty;
}