using CVA.Domain.Models;
using CVA.Infrastructure.Postgres;
using CVA.Tests.Common;
using CVA.Tests.Common.Comparers;
using CVA.Tests.Integration.Fixtures;

namespace CVA.Tests.Integration.Tests.Infrastructure.Postgres;

/// <summary>
/// Integration tests for the <see cref="UserPostgresRepository"/> using Testcontainers.
/// </summary>
[Trait(Layer.Infrastructure, Category.Repository)]
public sealed class UserPostgresRepositoryTests(PostgresFixture fixture) : PostgresTestBase(fixture)
{
    private static readonly UserComparer UserComp = new();

    /// <summary>
    /// Purpose: Verify repository persists a new user.
    /// When: CreateAsync is called with a valid user.
    /// Should: Persist the user and return it back from database with the same state.
    /// </summary>
    [Fact]
    public async Task CreateAsync_ShouldPersistUser()
    {
        // Arrange
        var user = DataGenerator.CreateUser();
        await using var context = CreateContext();
        var repository = CreateRepository(context);

        // Act
        var created = await repository.CreateAsync(user, Cts.Token);

        // Assert
        Assert.NotNull(created);
        var dbUser = await GetFreshUserAsync(created.Id, Cts.Token);
        Assert.NotNull(dbUser);
        Assert.Equal(user, dbUser, UserComp);
    }

    /// <summary>
    /// Purpose: Verify repository returns a user by id including work experience.
    /// When: GetByIdAsync is called for an existing user id.
    /// Should: Return the user with work experience loaded.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnUserWithWorkExperience()
    {
        // Arrange
        var seedUser = DataGenerator.CreateUser();
        var created = await SeedUserAsync(seedUser, Cts.Token);

        // Assert
        Assert.NotNull(created);

        await using var context = CreateContext();
        var repository = CreateRepository(context);

        // Act
        var result = await repository.GetByIdAsync(created.Id, Cts.Token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(seedUser, result, UserComp);
    }

    /// <summary>
    /// Purpose: Verify repository updates scalar fields and work experience.
    /// When: UpdateAsync is called for an existing user with modified fields.
    /// Should: Persist updated fields and replace work experience collection.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldUpdateFieldsAndWorkExperience()
    {
        // Arrange
        var initialUser = DataGenerator.CreateUser();
        var created = await SeedUserAsync(initialUser, Cts.Token);
        var newName = DataGenerator.CreateString();
        var newSurname = DataGenerator.CreateString();
        var newWork = Work.Create("Fact Corp");

        // Assert
        Assert.NotNull(created);

        created.ChangeName(newName, newSurname);
        created.ReplaceWorkExperience([newWork]);

        await using var context = CreateContext();
        var repository = CreateRepository(context);

        // Act
        await repository.UpdateAsync(created, Cts.Token);

        // Assert
        var dbUser = await GetFreshUserAsync(created.Id, Cts.Token);
        Assert.NotNull(dbUser);
        Assert.Equal(newName, dbUser.Name);
        Assert.Equal(newSurname, dbUser.Surname);
        Assert.Equal("Fact Corp", dbUser.WorkExperience.FirstOrDefault()?.CompanyName);
    }

    /// <summary>
    /// Purpose: Verify repository deletes a user.
    /// When: DeleteAsync is called for an existing user id.
    /// Should: Remove the user so it cannot be loaded afterwards.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser()
    {
        // Arrange
        var user = DataGenerator.CreateUser();
        var created = await SeedUserAsync(user, Cts.Token);

        await using var context = CreateContext();
        var repository = CreateRepository(context);

        //Assert
        Assert.NotNull(created);

        // Act
        await repository.DeleteAsync(created.Id, Cts.Token);

        // Assert
        var dbUser = await GetFreshUserAsync(created.Id, Cts.Token);
        Assert.Null(dbUser);
    }

    /// <summary>
    /// Purpose: Verify repository returns all users.
    /// When: GetAllAsync is called and multiple users exist.
    /// Should: Return the same amount of users as stored.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ShouldReturnUsers()
    {
        // Arrange
        var users = DataGenerator.CreateUsers(2).ToArray();
        foreach (var user in users)
            await SeedUserAsync(user, Cts.Token);

        await using var context = CreateContext();
        var repository = CreateRepository(context);

        // Act
        var result = await repository.GetAllAsync(Cts.Token);

        // Assert
        Assert.Equal(2, result.Count());
    }

    /// <summary>
    /// Purpose: Verify repository returns null for missing users.
    /// When: GetByIdAsync is called with a non-existent id.
    /// Should: Return null.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        await using var context = CreateContext();
        var repository = CreateRepository(context);

        // Act
        var result = await repository.GetByIdAsync(Guid.CreateVersion7(), Cts.Token);

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Purpose: Verify delete is idempotent.
    /// When: DeleteAsync is called with a non-existent id.
    /// Should: Not throw.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldNotThrow_WhenUserDoesNotExist()
    {
        // Arrange
        await using var context = CreateContext();
        var repository = CreateRepository(context);

        // Act
        var exception = await Record.ExceptionAsync(() => repository.DeleteAsync(Guid.CreateVersion7(), Cts.Token));

        // Assert
        Assert.Null(exception);
    }
}
