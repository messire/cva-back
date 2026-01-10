namespace CVA.Infrastructure.Postgres;

/// <summary>
/// PostgreSQL repository for DeveloperProfile aggregate.
/// </summary>
internal sealed class DeveloperProfilePostgresRepository(PostgresContext context) : IDeveloperProfileRepository
{
    /// <summary>
    /// Retrieves a developer profile by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the developer profile.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The developer profile associated with the specified identifier, or null if no match is found. </returns>
    public async Task<DeveloperProfile?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await context.DeveloperProfiles
            .AsSplitQuery()
            .Include(profileEntity => profileEntity.Projects)
            .Include(profileEntity => profileEntity.WorkExperience)
            .FirstOrDefaultAsync(profileEntity => profileEntity.Id == id, ct);

        return entity?.ToDomain();
    }

    /// <summary>
    /// Retrieves all developer profiles asynchronously.
    /// </summary>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A read-only collection of all developer profiles available in the repository.</returns>
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

    /// <summary>
    /// Creates a new developer profile asynchronously in the repository.
    /// </summary>
    /// <param name="profile">The developer profile to be created.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The newly created developer profile.</returns>
    public async Task<DeveloperProfile> CreateAsync(
        DeveloperProfile profile,
        CancellationToken ct)
    {
        var entity = profile.ToEntity();

        context.DeveloperProfiles.Add(entity);
        await context.SaveChangesAsync(ct);

        return entity.ToDomain();
    }

    /// <summary>
    /// Updates an existing developer profile in the database asynchronously.
    /// </summary>
    /// <param name="profile">The developer profile containing the updated data.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The updated developer profile if the update was successful, or null if the profile was not found.</returns>
    public async Task<DeveloperProfile?> UpdateAsync(
        DeveloperProfile profile,
        CancellationToken ct)
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

    /// <summary>
    /// Deletes a developer profile by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the developer profile to be deleted.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>True if the developer profile was successfully deleted; otherwise, false if no match is found.</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await context.DeveloperProfiles.FirstOrDefaultAsync(profileEntity => profileEntity.Id == id, ct);
        if (entity is null) return false;

        context.DeveloperProfiles.Remove(entity);
        await context.SaveChangesAsync(ct);
        return true;
    }
}