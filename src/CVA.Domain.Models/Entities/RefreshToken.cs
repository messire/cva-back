namespace CVA.Domain.Models;

/// <summary>
/// Represents a persisted refresh token for a user.
/// Refresh tokens are stored hashed (never store the raw token).
/// </summary>
public sealed class RefreshToken
{
    /// <summary>
    /// Refresh token identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The user who owns this refresh token.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Hash of the opaque refresh token value.
    /// </summary>
    public string TokenHash { get; private set; } = string.Empty;

    /// <summary>
    /// Refresh token expiration time (UTC).
    /// </summary>
    public DateTimeOffset ExpiresAt { get; private set; }

    /// <summary>
    /// Creation time (UTC).
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Revocation time (UTC). If set, the token is no longer valid.
    /// </summary>
    public DateTimeOffset? RevokedAt { get; private set; }

    /// <summary>
    /// Hash of the replacement token (rotation). Optional.
    /// </summary>
    public string? ReplacedByTokenHash { get; private set; }

    /// <summary>
    /// Indicates whether the token is currently active.
    /// </summary>
    /// <param name="now">The current timestamp.</param>
    public bool IsActive(DateTimeOffset now)
        => RevokedAt is null && ExpiresAt > now;

    /// <summary>
    /// EF constructor.
    /// </summary>
    private RefreshToken()
    {
    }

    private RefreshToken(
        Guid id,
        Guid userId,
        string tokenHash,
        DateTimeOffset expiresAt,
        DateTimeOffset createdAt,
        DateTimeOffset? revokedAt,
        string? replacedByTokenHash)
    {
        Id = id;
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        CreatedAt = createdAt;
        RevokedAt = revokedAt;
        ReplacedByTokenHash = replacedByTokenHash;
    }

    /// <summary>
    /// Creates a new refresh token record.
    /// </summary>
    /// <param name="userId">User id.</param>
    /// <param name="tokenHash">Hashed token value.</param>
    /// <param name="expiresAt">Expiration time (UTC).</param>
    /// <param name="now">The current timestamp.</param>
    public static RefreshToken Create(Guid userId, string tokenHash, DateTimeOffset expiresAt, DateTimeOffset now)
        => new(
            id: Guid.CreateVersion7(),
            userId: userId,
            tokenHash: tokenHash,
            expiresAt: expiresAt,
            createdAt: now,
            revokedAt: null,
            replacedByTokenHash: null);

    /// <summary>
    /// Restores a refresh token from persistence.
    /// </summary>
    /// <param name="id">Token id.</param>
    /// <param name="userId">User id.</param>
    /// <param name="tokenHash">Hashed token value.</param>
    /// <param name="expiresAt">Expiration time (UTC).</param>
    /// <param name="createdAt">Creation time (UTC).</param>
    /// <param name="revokedAt">Revocation time (UTC).</param>
    /// <param name="replacedByTokenHash">Replacement token hash.</param>
    public static RefreshToken FromPersistence(
        Guid id,
        Guid userId,
        string tokenHash,
        DateTimeOffset expiresAt,
        DateTimeOffset createdAt,
        DateTimeOffset? revokedAt,
        string? replacedByTokenHash)
        => new(id, userId, tokenHash, expiresAt, createdAt, revokedAt, replacedByTokenHash);

    /// <summary>
    /// Revokes the refresh token.
    /// </summary>
    /// <param name="revokedAt">Revocation time (UTC).</param>
    /// <param name="replacedByTokenHash">Replacement token hash.</param>
    public void Revoke(DateTimeOffset revokedAt, string? replacedByTokenHash)
    {
        RevokedAt = revokedAt;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}