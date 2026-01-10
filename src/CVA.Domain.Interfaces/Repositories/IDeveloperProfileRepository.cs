namespace CVA.Domain.Interfaces;

/// <summary>
/// Repository for managing DeveloperProfile aggregate.
/// </summary>
public interface IDeveloperProfileRepository
{
    /// <summary>
    /// Gets a DeveloperProfile by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the DeveloperProfile to retrieve.</param>
    /// <param name="ct">The cancellation token to use for the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the DeveloperProfile if found, otherwise null.</returns>
    Task<DeveloperProfile?> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Gets all DeveloperProfiles.
    /// </summary>
    /// <param name="ct">The cancellation token to use for the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only collection of DeveloperProfiles.</returns>
    Task<IReadOnlyCollection<DeveloperProfile>> GetAllAsync(CancellationToken ct);

    /// <summary>
    /// Creates a new DeveloperProfile.
    /// </summary>
    /// <param name="profile">The DeveloperProfile to create.</param>
    /// <param name="ct">The cancellation token to use for the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created DeveloperProfile.</returns>
    Task<DeveloperProfile> CreateAsync(DeveloperProfile profile, CancellationToken ct);

    /// <summary>
    /// Updates an existing DeveloperProfile.
    /// </summary>
    /// <param name="profile">The DeveloperProfile to update.</param>
    /// <param name="ct">The cancellation token to use for the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated DeveloperProfile.</returns>
    Task<DeveloperProfile?> UpdateAsync(DeveloperProfile profile, CancellationToken ct);
    /// <summary>
    /// Deletes a DeveloperProfile by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the DeveloperProfile to delete.</param>
    /// <param name="ct">The cancellation token to use for the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}