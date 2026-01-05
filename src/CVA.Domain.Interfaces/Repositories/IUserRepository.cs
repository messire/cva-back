using CVA.Domain.Models;

namespace CVA.Domain.Interfaces;

/// <summary>
/// Represents a repository interface for managing user data in the database.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds a new user to the database and saves the changes asynchronously.
    /// </summary>
    /// <param name="user">The user entity to be added to the database.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The newly created user entity after being saved in the database.</returns>
    Task<User?> CreateAsync(User user, CancellationToken ct);

    /// <summary>
    /// Retrieves a user from the database by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Retrieves all user entities from the database asynchronously.
    /// </summary>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A collection of all user entities in the database.</returns>
    Task<IEnumerable<User>> GetAllAsync(CancellationToken ct);

    /// <summary>
    /// Updates an existing user in the database and saves the changes asynchronously.
    /// </summary>
    /// <param name="user">The user entity with updated information to be saved to the database.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The updated user entity after being saved in the database, or null if the user was not found.</returns>
    Task<User?> UpdateAsync(User user, CancellationToken ct);

    /// <summary>
    /// Deletes a user from the database by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The deleted user entity if deletion was successful; otherwise, null.</returns>
    Task<User?> DeleteAsync(Guid id, CancellationToken ct);
}