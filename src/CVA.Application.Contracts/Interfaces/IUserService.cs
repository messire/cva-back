namespace CVA.Application.Contracts;

/// <summary>
/// Interface for operations related to user management.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a collection of user data transfer objects.
    /// </summary>
    /// <param name="ct">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an enumerable collection of <see cref="UserDto"/> if users are found.</returns>
    Task<Result<IEnumerable<UserDto>>> GetUsersAsync(CancellationToken ct);

    /// <summary>
    /// Retrieves a user data transfer object by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <param name="ct">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="UserDto"/> representing the user if found, or null if no user matches the specified identifier.</returns>
    Task<Result<UserDto>> GetUserByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Updates an existing user with the provided data transfer object.
    /// </summary>
    /// <param name="id">The unique identifier of the user to be updated.</param>
    /// <param name="user">The <see cref="UserDto"/> containing updated user information.</param>
    /// <param name="ct">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated <see cref="UserDto"/> if the update is successful, or null if the user does not exist.</returns>
    Task<Result<UserDto>> UpdateUserAsync(Guid id, UserDto user, CancellationToken ct);

    /// <summary>
    /// Deletes a user with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to be deleted.</param>
    /// <param name="ct">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the deleted <see cref="UserDto"/> if the operation is successful; otherwise, null.</returns>
    Task<Result<UserDto>> DeleteUserAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Creates a new user and returns the created user data transfer object.
    /// </summary>
    /// <param name="user">The user data transfer object containing the user's information to be created.</param>
    /// <param name="ct">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the created <see cref="UserDto"/> if the operation is successful; otherwise, null.</returns>
    Task<Result<UserDto>> CreateUserAsync(UserDto user, CancellationToken ct);
}