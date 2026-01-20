namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Mapping extensions for refresh tokens between domain and EF Core entities.
/// </summary>
internal static class RefreshTokenMappingExtensions
{
    /// <summary>
    /// Maps domain model to EF Core entity.
    /// </summary>
    /// <param name="token">Domain refresh token.</param>
    public static RefreshTokenEntity ToEntity(this RefreshToken token)
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
    /// Maps EF Core entity to domain model.
    /// </summary>
    /// <param name="entity">EF Core refresh token entity.</param>
    public static RefreshToken ToDomain(this RefreshTokenEntity entity)
        => RefreshToken.FromPersistence(
            id: entity.Id,
            userId: entity.UserId,
            tokenHash: entity.TokenHash,
            expiresAt: entity.ExpiresAt,
            createdAt: entity.CreatedAt,
            revokedAt: entity.RevokedAt,
            replacedByTokenHash: entity.ReplacedByTokenHash);
}