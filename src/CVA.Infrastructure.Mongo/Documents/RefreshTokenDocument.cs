namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Represents a MongoDB document for a refresh token.
/// </summary>
internal sealed class RefreshTokenDocument
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
    /// Hash of the opaque refresh token value.
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
    /// Revocation time (UTC). If set, the token is no longer valid.
    /// </summary>
    public DateTimeOffset? RevokedAt { get; set; }

    /// <summary>
    /// Hash of the replacement token, if rotated.
    /// </summary>
    public string? ReplacedByTokenHash { get; set; }
}