namespace CVA.Application.Services;

/// <summary>
/// Implements the <see cref="IUserService"/> interface.
/// </summary>
/// <param name="userRepository">The repository for user data.</param>
internal class UserService(IUserRepository userRepository) : IUserService
{
    /// <inheritdoc />
    public async Task<Result<IEnumerable<UserDto>>> GetUsersAsync(CancellationToken ct)
    {
        try
        {
            var users = await userRepository.GetAllAsync(ct);
            return Result<IEnumerable<UserDto>>.Ok(users.ToDto());
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            return Result<IEnumerable<UserDto>>.Fail(exception.Message);
        }
    }

    /// <inheritdoc />
    public async Task<Result<UserDto>> GetUserByIdAsync(Guid id, CancellationToken ct)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(id, ct);
            return user is null
                ? Result<UserDto>.Fail($"User with id '{id}' not found.")
                : Result<UserDto>.Ok(user.ToDto());
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            return Result<UserDto>.Fail(exception.Message);
        }
    }

    /// <inheritdoc />
    public async Task<Result<UserDto>> UpdateUserAsync(Guid id, UserDto updatedUser, CancellationToken ct)
    {
        if (id == Guid.Empty)
        {
            return Result<UserDto>.Fail($"Invalid route id '{id}' value.");
        }

        if (updatedUser.Id is null || updatedUser.Id == Guid.Empty)
        {
            return Result<UserDto>.Fail($"Invalid body id '{updatedUser.Id}' value.");
        }

        if (updatedUser.Id.Value != id)
        {
            return Result<UserDto>.Fail($"Route id '{id}' does not match body id '{updatedUser.Id}'.");
        }

        try
        {
            var user = await userRepository.GetByIdAsync((Guid)updatedUser.Id, ct);
            if (user is null)
            {
                return Result<UserDto>.Fail($"User with id '{updatedUser.Id}' not found.");
            }

            user.ChangeName(updatedUser.Name, updatedUser.Surname);
            user.ChangeEmail(updatedUser.Email);
            user.UpdateProfile(updatedUser.Phone, updatedUser.Birthday, updatedUser.SummaryInfo);
            user.UpdatePhoto(updatedUser.Photo);
            user.ReplaceSkills(updatedUser.Skills);
            user.ReplaceWorkExperience(updatedUser.WorkExperience?.ToModel());

            await userRepository.UpdateAsync(user, ct);
            return Result<UserDto>.Ok(user.ToDto());
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            return Result<UserDto>.Fail(exception.Message);
        }
    }

    /// <inheritdoc />
    public async Task<Result<UserDto>> DeleteUserAsync(Guid id, CancellationToken ct)
    {
        try
        {
            var user = await userRepository.DeleteAsync(id, ct);
            return user is null
                ? Result<UserDto>.Fail($"User with id '{id}' not found.")
                : Result<UserDto>.Ok(user.ToDto());
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            return Result<UserDto>.Fail(exception.Message);
        }
    }

    /// <inheritdoc />
    public async Task<Result<UserDto>> CreateUserAsync(UserDto userDto, CancellationToken ct)
    {
        try
        {
            var created = await userRepository.CreateAsync(userDto.ToModel(), ct);
            return created is null
                ? Result<UserDto>.Fail("Unable to create user.")
                : Result<UserDto>.Ok(created.ToDto());
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            return Result<UserDto>.Fail(exception.Message);
        }
    }
}