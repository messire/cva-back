namespace CVA.Infrastructure.Postgres;

/// <summary>
/// PostgreSQL repository for DeveloperProfile aggregate.
/// </summary>
internal sealed class DeveloperProfilePostgresRepository(PostgresContext context) : IDeveloperProfileRepository
{
    /// <inheritdoc/>
    public async Task<PagedResult<DeveloperProfile>> SearchCatalogAsync(ProfilesCatalogRequest request, CancellationToken ct)
    {
        var query = context.DeveloperProfiles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(entity => entity.FirstName.Contains(request.Search) || entity.LastName.Contains(request.Search));
        }

        if (request.OpenToWork.HasValue)
        {
            query = query.Where(entity => entity.OpenToWork == request.OpenToWork.Value);
        }

        if (request.VerificationStatus is not null)
        {
            query = query.Where(entity => entity.Verified == request.VerificationStatus.Value);
        }

        if (request.Skills is { Length: > 0 })
        {
            foreach (var skill in request.Skills)
            {
                query = query.Where(entity => entity.Skills.Contains(skill));
            }
        }

        var totalCount = await query.LongCountAsync(ct);

        query = ApplySorting(query, request.Sort);

        var skip = (request.Page.Number - 1) * request.Page.Size;

        var items = await query
            .Skip(skip)
            .Take(request.Page.Size)
            .Select(entity => entity.ToDomain())
            .ToArrayAsync(ct);

        return new PagedResult<DeveloperProfile>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    /// <inheritdoc/>
    public async Task<DeveloperProfile?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await context.DeveloperProfiles
            .AsSplitQuery()
            .Include(profileEntity => profileEntity.Projects)
            .Include(profileEntity => profileEntity.WorkExperience)
            .FirstOrDefaultAsync(profileEntity => profileEntity.Id == id, ct);

        return entity?.ToDomain();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<DeveloperProfile>> GetAllAsync(CancellationToken ct)
    {
        var entities = await context.DeveloperProfiles
            .AsSplitQuery()
            .Include(profileEntity => profileEntity.Projects)
            .Include(profileEntity => profileEntity.WorkExperience)
            .ToListAsync(ct);

        return entities
            .Select(profileEntity => profileEntity.ToDomain())
            .ToArray();
    }

    /// <inheritdoc/>
    public async Task<DeveloperProfile> CreateAsync(DeveloperProfile profile, CancellationToken ct)
    {
        var entity = profile.ToEntity();

        context.DeveloperProfiles.Add(entity);
        await context.SaveChangesAsync(ct);

        return entity.ToDomain();
    }

    /// <inheritdoc/>
    public async Task<DeveloperProfile?> UpdateAsync(DeveloperProfile profile, CancellationToken ct)
    {
        var entity = await context.DeveloperProfiles
            .AsSplitQuery()
            .Include(profileEntity => profileEntity.Projects)
            .Include(profileEntity => profileEntity.WorkExperience)
            .FirstOrDefaultAsync(profileEntity => profileEntity.Id == profile.Id.Value, ct);

        if (entity is null) return null;

        entity.UpdateFromDomain(profile);
        await context.SaveChangesAsync(ct);
        return entity.ToDomain();
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await context.DeveloperProfiles.FirstOrDefaultAsync(profileEntity => profileEntity.Id == id, ct);
        if (entity is null) return false;

        context.DeveloperProfiles.Remove(entity);
        await context.SaveChangesAsync(ct);
        return true;
    }

    private static IQueryable<DeveloperProfileEntity> ApplySorting(
        IQueryable<DeveloperProfileEntity> query,
        ProfilesCatalogSort sort)
    {
        return sort.Field switch
        {
            ProfilesSortField.UpdatedAt => sort.Order == SortOrder.Desc
                ? query.OrderByDescending(entity => entity.UpdatedAt)
                    .ThenByDescending(entity => entity.Id)
                : query.OrderBy(entity => entity.UpdatedAt)
                    .ThenBy(entity => entity.Id),

            ProfilesSortField.Name => sort.Order == SortOrder.Desc
                ? query.OrderByDescending(entity => entity.LastName)
                    .ThenByDescending(entity => entity.FirstName)
                    .ThenByDescending(entity => entity.Id)
                : query.OrderBy(entity => entity.LastName)
                    .ThenBy(entity => entity.FirstName)
                    .ThenBy(entity => entity.Id),

            ProfilesSortField.Id => sort.Order == SortOrder.Desc
                ? query.OrderByDescending(entity => entity.Id)
                : query.OrderBy(entity => entity.Id),

            _ => query.OrderByDescending(entity => entity.UpdatedAt)
                .ThenByDescending(entity => entity.Id)
        };
    }
}