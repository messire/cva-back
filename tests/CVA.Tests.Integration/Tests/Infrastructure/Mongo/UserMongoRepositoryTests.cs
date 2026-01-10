using CVA.Infrastructure.Mongo;
using CVA.Tests.Common.Comparers;
using MongoDB.Driver;

namespace CVA.Tests.Integration.Tests.Infrastructure.Mongo;

/// <summary>
/// Integration tests for the <see cref="UserMongoRepository"/> using Testcontainers.
/// </summary>
[Trait(Layer.Infrastructure, Category.Repository)]
public sealed class UserMongoRepositoryTests(MongoFixture fixture) : MongoTestBase(fixture)
{
    private static readonly UserComparer UserComp = new();

    /// <summary>
    /// Purpose: Verify persistence of a new user including embedded work experience.
    /// When: Creating a user via repository.
    /// Should: Persist full document and allow reading the same domain state back.
    /// </summary>
    [Fact]
    public async Task CreateAsync_ShouldPersistUser()
    {
        // Arrange
        var user = DataGenerator.CreateUser();
        var repository = CreateUserRepository();

        // Act
        var result = await repository.CreateAsync(user, Cts.Token);

        // Assert
        Assert.NotNull(result);
        var dbUser = await GetFreshUser(result.Id);
        Assert.NotNull(dbUser);
        Assert.Equal(user, dbUser, UserComp);
    }

    /// <summary>
    /// Purpose: Verify retrieval of a user by id.
    /// When: The document exists in Mongo.
    /// Should: Return the corresponding domain user (including work experience).
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser()
    {
        // Arrange
        var seedUser = DataGenerator.CreateUser();
        await GetCollection().InsertOneAsync(seedUser.ToDocument(), cancellationToken: Cts.Token);

        var repository = CreateUserRepository();

        // Act
        var result = await repository.GetByIdAsync(seedUser.Id, Cts.Token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(seedUser, result, UserComp);
    }

    /// <summary>
    /// Purpose: Verify missing user returns null.
    /// When: Document with the given id does not exist.
    /// Should: Return null.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var repository = CreateUserRepository();

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid(), Cts.Token);

        // Assert
        Assert.Null(result);
    }

    private IMongoCollection<UserDocument> GetCollection()
        => MongoClient.GetDatabase(MongoOptions.DatabaseName).GetCollection<UserDocument>("users");

    private async Task<User?> GetFreshUser(Guid id)
    {
        var userDocument = await GetCollection()
            .Find(document => document.Id == id)
            .FirstOrDefaultAsync(Cts.Token);

        return userDocument?.ToDomain();
    }
}