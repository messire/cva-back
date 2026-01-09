using CVA.Infrastructure.Postgres;
using CVA.Tests.Common.Comparers;

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
}
