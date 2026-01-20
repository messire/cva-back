namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Provides an implementation of the <see cref="IRefreshTokenRepository"/> interface for interacting with refresh tokens in a PostgreSQL database.
/// </summary>
internal sealed class RefreshTokenPostgresRepository(PostgresContext context) : IRefreshTokenRepository
{
    /// <inheritdoc />
    public async Task CreateAsync(RefreshToken refreshToken, CancellationToken ct)
    {
        var entity = refreshToken.ToEntity();
        await context.RefreshTokens.AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);
    }

    /// <inheritdoc />
    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct)
    {
        var entity = await context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(tokenEntity => tokenEntity.TokenHash == tokenHash, ct);

        return entity?.ToDomain();
    }

    /// <inheritdoc />
    public async Task RevokeAsync(Guid id, DateTimeOffset revokedAt, string? replacedByTokenHash, CancellationToken ct)
    {
        var entity = await context.RefreshTokens.FirstOrDefaultAsync(tokenEntity => tokenEntity.Id == id, ct);
        if (entity is null) return;

        entity.RevokedAt = revokedAt;
        entity.ReplacedByTokenHash = replacedByTokenHash;
        await context.SaveChangesAsync(ct);
    }
}