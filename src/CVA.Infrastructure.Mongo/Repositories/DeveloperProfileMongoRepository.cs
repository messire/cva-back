namespace CVA.Infrastructure.Mongo;

/// <summary>
/// MongoDB repository for DeveloperProfile aggregate.
/// </summary>
internal sealed class DeveloperProfileMongoRepository(IMongoClient client, MongoOptions options) : IDeveloperProfileRepository
{
    private readonly IMongoCollection<DeveloperProfileDocument> _profiles = client
        .GetDatabase(options.DatabaseName)
        .GetCollection<DeveloperProfileDocument>("developer_profiles");

    /// <inheritdoc/>
    public async Task<DeveloperProfile> CreateAsync(DeveloperProfile profile, CancellationToken ct)
    {
        var document = profile.ToDocument();
        await _profiles.InsertOneAsync(document, cancellationToken: ct);
        return profile;
    }

    /// <inheritdoc/>
    public async Task<DeveloperProfile?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var document = await _profiles.Find(profileDocument => profileDocument.Id == id).FirstOrDefaultAsync(ct);
        return document?.ToDomain();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<DeveloperProfile>> GetAllAsync(CancellationToken ct)
    {
        var documents = await _profiles.Find(_ => true).ToListAsync(ct);
        return documents.Select(document => document.ToDomain()).ToArray();
    }

    /// <inheritdoc/>
    public async Task<DeveloperProfile?> UpdateAsync(DeveloperProfile profile, CancellationToken ct)
    {
        var document = profile.ToDocument();
        var options = new FindOneAndReplaceOptions<DeveloperProfileDocument> { ReturnDocument = ReturnDocument.After };

        var updated = await _profiles.FindOneAndReplaceAsync(
            profileDocument => profileDocument.Id == document.Id,
            document,
            options,
            ct);

        return updated?.ToDomain();
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var deleted = await _profiles.FindOneAndDeleteAsync(document => document.Id == id, cancellationToken: ct);
        return deleted is not null;
    }
}