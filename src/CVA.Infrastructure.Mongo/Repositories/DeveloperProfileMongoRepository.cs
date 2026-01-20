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
    public async Task<PagedResult<DeveloperProfile>> SearchCatalogAsync(ProfilesCatalogRequest request, CancellationToken ct)
    {
        var filter = BuildFilter(request);
        var sort = BuildSort(request.Sort);
        var totalCount = await _profiles.CountDocumentsAsync(filter, cancellationToken: ct);
        var skip = (request.Page.Number - 1) * request.Page.Size;

        var docs = await _profiles.Find(filter)
            .Sort(sort).Skip(skip).Limit(request.Page.Size)
            .ToListAsync(ct);

        return new PagedResult<DeveloperProfile>
        {
            Items = docs.Select(document => document.ToDomain()).ToArray(),
            TotalCount = totalCount
        };
    }
    
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
    
    private static FilterDefinition<DeveloperProfileDocument> BuildFilter(ProfilesCatalogRequest request)
    {
        var builder = Builders<DeveloperProfileDocument>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            filter &= builder.Or(
                builder.Regex(document => document.FirstName, request.Search),
                builder.Regex(document => document.LastName, request.Search));
        }

        if (request.OpenToWork.HasValue)
        {
            filter &= builder.Eq(document => document.OpenToWork, request.OpenToWork.Value);
        }

        if (request.VerificationStatus is not null)
        {
            filter &= builder.Eq(document => document.VerificationStatus, (int)request.VerificationStatus.Value);
        }

        if (request.Skills is { Length: > 0 })
        {
            filter &= builder.All(document => document.Skills, request.Skills);
        }

        return filter;
    }
    
    private static SortDefinition<DeveloperProfileDocument> BuildSort(ProfilesCatalogSort sort)
    {
        var builder = Builders<DeveloperProfileDocument>.Sort;

        return sort.Field switch
        {
            ProfilesSortField.UpdatedAt => sort.Order == SortOrder.Desc
                ? builder.Descending(document => document.UpdatedAt)
                    .Descending(document => document.Id)
                : builder.Ascending(document => document.UpdatedAt)
                    .Ascending(document => document.Id),

            ProfilesSortField.Name => sort.Order == SortOrder.Desc
                ? builder.Descending(document => document.LastName)
                    .Descending(document => document.FirstName)
                    .Descending(document => document.Id)
                : builder.Ascending(document => document.LastName)
                    .Ascending(document => document.FirstName)
                    .Ascending(document => document.Id),

            ProfilesSortField.Id => sort.Order == SortOrder.Desc
                ? builder.Descending(document => document.Id)
                : builder.Ascending(document => document.Id),

            _ => builder.Descending(document => document.UpdatedAt).Descending(document => document.Id)
        };
    }
}