namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Mapping extensions for refresh tokens between domain and MongoDB documents.
/// </summary>
internal static class RefreshTokenMappingExtensions
{
    /// <summary>
    /// Maps a domain refresh token to a MongoDB document.
    /// </summary>
    /// <param name="token">Domain refresh token.</param>
    public static RefreshTokenDocument ToDocument(this RefreshToken token)
        => new()
        {
            Id = token.Id,
            UserId = token.UserId,
            TokenHash = token.TokenHash,
            ExpiresAt = token.ExpiresAt,
            CreatedAt = token.CreatedAt,
            RevokedAt = token.RevokedAt,
            ReplacedByTokenHash = token.ReplacedByTokenHash
        };

    /// <summary>
    /// Maps a MongoDB refresh token document to a domain model.
    /// </summary>
    /// <param name="document">MongoDB refresh token document.</param>
    public static RefreshToken ToDomain(this RefreshTokenDocument document)
        => RefreshToken.FromPersistence(
            id: document.Id,
            userId: document.UserId,
            tokenHash: document.TokenHash,
            expiresAt: document.ExpiresAt,
            createdAt: document.CreatedAt,
            revokedAt: document.RevokedAt,
            replacedByTokenHash: document.ReplacedByTokenHash);
}