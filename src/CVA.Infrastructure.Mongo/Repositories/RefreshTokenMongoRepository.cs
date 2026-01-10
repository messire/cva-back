namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Represents a MongoDB repository for managing refresh tokens.
/// Implements the <see cref="IRefreshTokenRepository"/> interface.
/// </summary>
internal sealed class RefreshTokenMongoRepository(IMongoClient client, MongoOptions options) : IRefreshTokenRepository
{
    private const string CollectionName = "refresh_tokens";

    private readonly IMongoCollection<RefreshTokenDocument> _tokens = client
        .GetDatabase(options.DatabaseName)
        .GetCollection<RefreshTokenDocument>(CollectionName);

    /// <summary>
    /// Ensures required indexes for refresh tokens exist.
    /// </summary>
    /// <remarks>
    /// Index creation in MongoDB is idempotent: it is safe to call multiple times.
    /// </remarks>
    private void EnsureIndexes()
    {
        var tokenHashIndex = new CreateIndexModel<RefreshTokenDocument>(
            keys: Builders<RefreshTokenDocument>.IndexKeys.Ascending(document => document.TokenHash),
            options: new CreateIndexOptions { Unique = true, Name = "ux_refresh_tokens_token_hash" });

        var userIdIndex = new CreateIndexModel<RefreshTokenDocument>(
            keys: Builders<RefreshTokenDocument>.IndexKeys.Ascending(document => document.UserId),
            options: new CreateIndexOptions { Unique = false, Name = "ix_refresh_tokens_user_id" });

        _tokens.Indexes.CreateMany([tokenHashIndex, userIdIndex]);
    }

    /// <inheritdoc />
    public async Task CreateAsync(RefreshToken refreshToken, CancellationToken ct)
    {
        EnsureIndexes();

        var document = refreshToken.ToDocument();
        await _tokens.InsertOneAsync(document, cancellationToken: ct);
    }

    /// <inheritdoc />
    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct)
    {
        EnsureIndexes();

        var tokenDocument = await _tokens.Find(document => document.TokenHash == tokenHash).FirstOrDefaultAsync(ct);
        return tokenDocument?.ToDomain();
    }

    /// <inheritdoc />
    public async Task RevokeAsync(Guid id, DateTimeOffset revokedAt, string? replacedByTokenHash, CancellationToken ct)
    {
        EnsureIndexes();

        var update = Builders<RefreshTokenDocument>.Update
            .Set(document => document.RevokedAt, revokedAt)
            .Set(document => document.ReplacedByTokenHash, replacedByTokenHash);

        await _tokens.UpdateOneAsync(document => document.Id == id, update, cancellationToken: ct);
    }
}