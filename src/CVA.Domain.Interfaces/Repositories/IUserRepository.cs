namespace CVA.Domain.Interfaces;

/// <summary>
/// Represents a repository interface for managing user accounts.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user">User to create.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<User?> CreateAsync(User user, CancellationToken ct);

    /// <summary>
    /// Gets a user by identifier.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Gets a user by Google subject identifier.
    /// </summary>
    /// <param name="googleSubject">Google subject identifier (<c>sub</c>).</param>
    /// <param name="ct">Cancellation token.</param>
    Task<User?> GetByGoogleSubjectAsync(string googleSubject, CancellationToken ct);

    /// <summary>
    /// Updates the user role.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="role">New role.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<User?> UpdateRoleAsync(Guid id, string role, CancellationToken ct);
}