using CVA.Domain.Models;
using CVA.Infrastructure.Mongo;
using CVA.Infrastructure.Mongo.Documents;
using CVA.Infrastructure.Mongo.Mapping;
using CVA.Tests.Common;
using CVA.Tests.Common.Comparers;
using CVA.Tests.Integration.Fixtures;
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
        var repository = CreateRepository();

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

        var repository = CreateRepository();

        // Act
        var result = await repository.GetByIdAsync(seedUser.Id, Cts.Token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(seedUser, result, UserComp);
    }

    /// <summary>
    /// Purpose: Verify update replaces stored document state.
    /// When: Updating scalar fields and work experience.
    /// Should: Persist new values and return them on subsequent reads.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldUpdateFields()
    {
        // Arrange
        var initialUser = DataGenerator.CreateUser();
        await GetCollection().InsertOneAsync(initialUser.ToDocument(), cancellationToken: Cts.Token);

        var repository = CreateRepository();
        var newName = DataGenerator.CreateString();
        var newSurname = DataGenerator.CreateString();

        initialUser.ChangeName(newName, newSurname);
        initialUser.ReplaceWorkExperience([Work.Create("Mongo Corp")]);

        // Act
        await repository.UpdateAsync(initialUser, Cts.Token);

        // Assert
        var dbUser = await GetFreshUser(initialUser.Id);
        Assert.NotNull(dbUser);
        Assert.Equal(newName, dbUser.Name);
        Assert.Equal(newSurname, dbUser.Surname);
        Assert.Equal("Mongo Corp", dbUser.WorkExperience.FirstOrDefault()?.CompanyName);
    }

    /// <summary>
    /// Purpose: Verify deletion removes the stored document.
    /// When: Deleting an existing user.
    /// Should: Return null on subsequent reads.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser()
    {
        // Arrange
        var user = DataGenerator.CreateUser();
        await GetCollection().InsertOneAsync(user.ToDocument(), cancellationToken: Cts.Token);

        var repository = CreateRepository();

        // Act
        await repository.DeleteAsync(user.Id, Cts.Token);

        // Assert
        var dbUser = await GetFreshUser(user.Id);
        Assert.Null(dbUser);
    }

    /// <summary>
    /// Purpose: Verify retrieving all users returns all stored documents.
    /// When: Multiple documents exist in collection.
    /// Should: Return the same count of domain users.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ShouldReturnUsers()
    {
        // Arrange
        var users = DataGenerator.CreateUsers(2).ToList();
        await GetCollection().InsertManyAsync(users.Select(x => x.ToDocument()), cancellationToken: Cts.Token);

        var repository = CreateRepository();

        // Act
        var result = await repository.GetAllAsync(Cts.Token);

        // Assert
        Assert.Equal(2, result.Count());
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
        var repository = CreateRepository();

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid(), Cts.Token);

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Purpose: Verify delete is idempotent.
    /// When: Deleting a non-existing user.
    /// Should: Not throw.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldNotThrow_WhenUserDoesNotExist()
    {
        // Arrange
        var repository = CreateRepository();

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => repository.DeleteAsync(Guid.NewGuid(), Cts.Token));
        Assert.Null(exception);
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