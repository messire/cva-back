namespace CVA.Infrastructure.Postgres;

/// <summary>
/// EF Core persistence model for refresh tokens.
/// </summary>
internal sealed class RefreshTokenEntity
{
    /// <summary>
    /// Refresh token identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Owner user identifier.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Hash of the refresh token value.
    /// </summary>
    public string TokenHash { get; set; } = null!;

    /// <summary>
    /// Expiration time (UTC).
    /// </summary>
    public DateTimeOffset ExpiresAt { get; set; }

    /// <summary>
    /// Creation time (UTC).
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Revocation time (UTC).
    /// </summary>
    public DateTimeOffset? RevokedAt { get; set; }

    /// <summary>
    /// Hash of the replacement token, if rotated.
    /// </summary>
    public string? ReplacedByTokenHash { get; set; }
}