using CVA.Application.Services;
using CVA.Domain.Interfaces;
using CVA.Domain.Models;
using Moq;

namespace CVA.Tests.Unit.Application.Services;

/// <summary>
/// Unit tests for the <see cref="UserService"/> class.
/// </summary>
[Trait(Layer.Application, Category.Services)]
public class UserServiceTests
{
    /// <summary>
    /// Purpose: Verify that GetUsersAsync returns a collection of DTOs.
    /// Should: Return all users mapped to DTOs.
    /// When: Repository contains users.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task GetUsersAsync_Should_Return_All_Users(
        List<User> users,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await sut.GetUsersAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(users.Count, result.Value.Count());
        userRepositoryMock.Verify(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Purpose: Verify that GetUserByIdAsync returns a DTO when a user is found.
    /// Should: Call repository GetByIdAsync once and map the result to DTO.
    /// When: A valid Guid is provided and user exists in the database.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task GetUserByIdAsync_Should_Return_UserDto_When_User_Exists(
        User user,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await sut.GetUserByIdAsync(user.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(user.Id, result.Value.Id);
        Assert.Equal(user.Name, result.Value.Name);
        userRepositoryMock.Verify(repository => repository.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Purpose: Verify that GetUserByIdAsync returns null when user is not found.
    /// Should: Return null without throwing an exception.
    /// When: Repository returns null for the given ID.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task GetUserByIdAsync_Should_Return_Null_When_User_Does_Not_Exist(
        Guid userId,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await sut.GetUserByIdAsync(userId, CancellationToken.None);

        // Assert
        Assert.Null(result.Value);
        userRepositoryMock.Verify(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Purpose: Verify that CreateUserAsync correctly saves a new user.
    /// Should: Map DTO to Model, call CreateAsync and return the resulting DTO.
    /// When: A valid UserDto is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task CreateUserAsync_Should_Return_Created_UserDto(
        UserDto inputDto,
        User createdModel,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdModel);

        // Act
        var result = await sut.CreateUserAsync(inputDto, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(createdModel.Name, result.Value.Name);
        userRepositoryMock.Verify(repository => repository.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Purpose: Verify that UpdateUserAsync updates an existing user.
    /// Should: Map DTO to Model, call UpdateAsync and return the updated DTO.
    /// When: Valid UserDto with existing ID is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task UpdateUserAsync_Should_Return_Updated_UserDto(
        UserDto updateDto,
        User existingUser,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        var userId = updateDto.Id ?? Guid.NewGuid();
        updateDto = updateDto with { Id = userId };

        existingUser = User.FromPersistence(
            id: userId,
            name: existingUser.Name,
            surname: existingUser.Surname,
            email: existingUser.Email,
            phone: existingUser.Phone,
            photo:existingUser.Photo,
            birthday: existingUser.Birthday,
            summaryInfo: existingUser.SummaryInfo,
            skills: existingUser.Skills,
            workExperience: existingUser.WorkExperience
        );

        userRepositoryMock
            .Setup(userRepository => userRepository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        userRepositoryMock
            .Setup(userRepository => userRepository.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User user, CancellationToken _) => user);

        // Act
        var result = await sut.UpdateUserAsync(userId, updateDto, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(userId, result.Value.Id);

        userRepositoryMock.Verify(userRepository => userRepository.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        userRepositoryMock.Verify(userRepository => userRepository.UpdateAsync(It.Is<User>(user =>
                user.Id == userId &&
                user.Name == updateDto.Name &&
                user.Surname == updateDto.Surname &&
                user.Email == updateDto.Email
            ), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Purpose: Ensure DeleteUserAsync triggers repository deletion.
    /// Should: Call DeleteAsync and return DTO of the deleted user.
    /// When: Delete is requested for an existing user.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task DeleteUserAsync_Should_Return_Deleted_UserDto(
        Guid userId,
        User existingUser,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        var deletedUser = User.FromPersistence(
            id: userId,
            name: existingUser.Name,
            surname: existingUser.Surname,
            email: existingUser.Email,
            phone: existingUser.Phone,
            photo: existingUser.Photo,
            birthday: existingUser.Birthday,
            summaryInfo: existingUser.SummaryInfo,
            skills: existingUser.Skills,
            workExperience: existingUser.WorkExperience
        );

        userRepositoryMock
            .Setup(userRepository => userRepository.DeleteAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(deletedUser);

        // Act
        var result = await sut.DeleteUserAsync(userId, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(userId, result.Value.Id);
        userRepositoryMock.Verify(userRepository => userRepository.DeleteAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Purpose: Verify that OperationCanceledException is rethrown by UserService.
    /// Should: Rethrow the exception without catching it.
    /// When: Repository throws OperationCanceledException.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task GetUsersAsync_Should_Rethrow_OperationCanceledException(
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => sut.GetUsersAsync(CancellationToken.None));
    }

    /// <summary>
    /// Purpose: Verify that GetUserByIdAsync returns a failed result when an exception occurs.
    /// Should: Catch the exception and return Fail result with error message.
    /// When: Repository throws an exception.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task GetUserByIdAsync_Should_Return_Fail_On_Exception(
        Guid userId,
        string errorMessage,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(errorMessage));

        // Act
        var result = await sut.GetUserByIdAsync(userId, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(errorMessage, result.Error?.Message);
    }

    /// <summary>
    /// Purpose: Verify that UpdateUserAsync returns a failed result when a DomainValidationException occurs.
    /// Should: Catch the exception and return Fail result with error message.
    /// When: Domain logic throws DomainValidationException.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task UpdateUserAsync_Should_Return_Fail_On_DomainValidationException(
        UserDto updateDto,
        User existingUser,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        var userId = updateDto.Id ?? Guid.NewGuid();
        updateDto = updateDto with { Id = userId, Name = "" }; // Trigger validation error

        userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        // Act
        var result = await sut.UpdateUserAsync(userId, updateDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("name", result.Error?.Message);
    }

    /// <summary>
    /// Purpose: Verify that UpdateUserAsync rethrows OperationCanceledException from GetByIdAsync.
    /// Should: Rethrow the exception without catching it.
    /// When: GetByIdAsync throws OperationCanceledException.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task UpdateUserAsync_Should_Rethrow_OperationCanceledException_From_GetByIdAsync(
        UserDto updateDto,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        var userId = updateDto.Id ?? Guid.NewGuid();
        updateDto = updateDto with { Id = userId };

        userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => sut.UpdateUserAsync(userId, updateDto, CancellationToken.None));
    }

    /// <summary>
    /// Purpose: Verify that UpdateUserAsync returns a failed result when the user is not found.
    /// Should: Return Fail result with a "not found" message.
    /// When: Repository returns null for the given ID.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task UpdateUserAsync_Should_Return_Fail_When_User_Not_Found(
        UserDto updateDto,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        var userId = updateDto.Id ?? Guid.NewGuid();
        updateDto = updateDto with { Id = userId };

        userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await sut.UpdateUserAsync(userId, updateDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"User with id '{userId}' not found.", result.Error?.Message);
    }

    /// <summary>
    /// Purpose: Verify that UpdateUserAsync returns a failed result when the ID is invalid.
    /// Should: Return Fail result with an "invalid id" message.
    /// When: ID is null or empty.
    /// </summary>
    [Theory]
    [InlineCvaAutoData]
    public async Task UpdateUserAsync_Should_Return_Fail_When_Id_Is_Invalid(
        UserDto updateDto,
        IUserService sut)
    {
        // Arrange
        var id = Guid.Parse("00000000-0000-0000-0000-000000000000");
        updateDto = updateDto with { Id = id };

        // Act
        var result = await sut.UpdateUserAsync(id, updateDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Invalid route id '{updateDto.Id}' value.", result.Error?.Message);
    }

    /// <summary>
    /// Purpose: Verify that GetUsersAsync returns a failed result when an exception occurs.
    /// Should: Catch the exception and return Fail result with error message.
    /// When: Repository throws an exception.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task GetUsersAsync_Should_Return_Fail_On_Exception(
        string errorMessage,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(errorMessage));

        // Act
        var result = await sut.GetUsersAsync(CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(errorMessage, result.Error?.Message);
    }

    /// <summary>
    /// Purpose: Verify that GetUserByIdAsync rethrows OperationCanceledException.
    /// Should: Rethrow the exception without catching it.
    /// When: Repository throws OperationCanceledException.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task GetUserByIdAsync_Should_Rethrow_OperationCanceledException(
        Guid userId,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => sut.GetUserByIdAsync(userId, CancellationToken.None));
    }

    /// <summary>
    /// Purpose: Verify that DeleteUserAsync rethrows OperationCanceledException.
    /// Should: Rethrow the exception without catching it.
    /// When: Repository throws OperationCanceledException.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task DeleteUserAsync_Should_Rethrow_OperationCanceledException(
        Guid userId,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.DeleteAsync(userId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => sut.DeleteUserAsync(userId, CancellationToken.None));
    }

    /// <summary>
    /// Purpose: Verify that DeleteUserAsync returns a failed result when an exception occurs.
    /// Should: Catch the exception and return Fail result with error message.
    /// When: Repository throws an exception.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task DeleteUserAsync_Should_Return_Fail_On_Exception(
        Guid userId,
        string errorMessage,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.DeleteAsync(userId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(errorMessage));

        // Act
        var result = await sut.DeleteUserAsync(userId, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(errorMessage, result.Error?.Message);
    }

    /// <summary>
    /// Purpose: Verify that CreateUserAsync rethrows OperationCanceledException.
    /// Should: Rethrow the exception without catching it.
    /// When: Repository throws OperationCanceledException.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task CreateUserAsync_Should_Rethrow_OperationCanceledException(
        UserDto inputDto,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => sut.CreateUserAsync(inputDto, CancellationToken.None));
    }

    /// <summary>
    /// Purpose: Verify that CreateUserAsync returns a failed result when an exception occurs.
    /// Should: Catch the exception and return Fail result with error message.
    /// When: Repository throws an exception.
    /// </summary>
    [Theory, CvaAutoData]
    public async Task CreateUserAsync_Should_Return_Fail_On_Exception(
        UserDto inputDto,
        string errorMessage,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        IUserService sut)
    {
        // Arrange
        userRepositoryMock
            .Setup(repository => repository.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(errorMessage));

        // Act
        var result = await sut.CreateUserAsync(inputDto, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(errorMessage, result.Error?.Message);
    }
}