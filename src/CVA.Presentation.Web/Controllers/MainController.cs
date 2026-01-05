using Microsoft.AspNetCore.Authorization;

namespace CVA.Presentation.Web;

/// <summary>
/// The MainController class provides endpoints for managing user-related operations.
/// </summary>
[ApiController]
[Route("/api/users")]
[AllowAnonymous]
public sealed class MainController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Retrieves a collection of user data asynchronously.
    /// </summary>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A collection of <see cref="UserDto"/> objects.</returns>
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync(CancellationToken ct)
    {
        var result = await userService.GetUsersAsync(ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Retrieves information about a user by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <param name="ct">A cancellation token to observe while awaiting the operation.</param>
    /// <returns>A <see cref="UserDto"/> if found.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUserByIdAsync(Guid id, CancellationToken ct)
    {
        var result = await userService.GetUserByIdAsync(id, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="newUser">The user to create.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The created <see cref="UserDto"/>.</returns>
    [HttpPost("")]
    public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] UserDto newUser, CancellationToken ct)
    {
        var result = await userService.CreateUserAsync(newUser, ct);

        // Returns 201 Created on success with Location header pointing to GetUserByIdAsync.
        return this.ToCreatedAtActionResult(
            result,
            actionName: nameof(GetUserByIdAsync),
            routeValues: result.IsSuccess ? new { id = result.Value!.Id } : null);
    }

    /// <summary>
    /// Updates an existing user's information asynchronously.
    /// </summary>
    /// <param name="updatedUser">The updated user data.</param>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The updated <see cref="UserDto"/>.</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserDto>> UpdateUserAsync([FromBody] UserDto updatedUser, Guid id, CancellationToken ct)
    {
        var result = await userService.UpdateUserAsync(id, updatedUser, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Deletes a user identified by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The deleted <see cref="UserDto"/>.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<UserDto>> DeleteUserAsync(Guid id, CancellationToken ct)
    {
        var result = await userService.DeleteUserAsync(id, ct);
        return this.ToActionResult(result);
    }
}
