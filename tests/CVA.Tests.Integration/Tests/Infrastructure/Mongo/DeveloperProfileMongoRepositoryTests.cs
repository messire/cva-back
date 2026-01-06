using CVA.Domain.Models;
using CVA.Infrastructure.Mongo;
using CVA.Tests.Common;
using CVA.Tests.Common.Comparers;
using CVA.Tests.Integration.Fixtures;
using MongoDB.Driver;

namespace CVA.Tests.Integration.Tests.Infrastructure.Mongo;

/// <summary>
/// Integration tests for the <see cref="DeveloperProfileMongoRepository"/> using Testcontainers.
/// </summary>
[Trait(Layer.Infrastructure, Category.Repository)]
public sealed class DeveloperProfileMongoRepositoryTests(MongoFixture fixture) : MongoTestBase(fixture)
{
    private static readonly DeveloperProfileComparer ProfileComp = new();

    /// <summary>
    /// Purpose: Verify persistence of a new developer profile.
    /// When: Creating a profile via repository.
    /// Should: Persist the full document and allow reading the same domain state back.
    /// </summary>
    [Fact]
    public async Task CreateAsync_ShouldPersistProfile()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile();
        var repository = CreateDevProfileRepository();

        // Act
        var result = await repository.CreateAsync(profile, Cts.Token);

        // Assert
        Assert.NotNull(result);
        var dbProfile = await GetFreshProfile(result.Id.Value);
        Assert.NotNull(dbProfile);
        Assert.Equal(profile, dbProfile, ProfileComp);
    }

    /// <summary>
    /// Purpose: Verify retrieval of a developer profile by id.
    /// When: The document exists in Mongo.
    /// Should: Return the corresponding domain profile.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnProfile()
    {
        // Arrange
        var seedProfile = DataGenerator.CreateDeveloperProfile();
        await GetCollection().InsertOneAsync(seedProfile.ToDocument(), cancellationToken: Cts.Token);

        var repository = CreateDevProfileRepository();

        // Act
        var result = await repository.GetByIdAsync(seedProfile.Id.Value, Cts.Token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(seedProfile, result, ProfileComp);
    }

    /// <summary>
    /// Purpose: Verify update replaces stored document state.
    /// When: Updating fields like role title.
    /// Should: Persist new values and return them on subsequent reads.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldUpdateFields()
    {
        // Arrange
        var initialProfile = DataGenerator.CreateDeveloperProfile();
        await GetCollection().InsertOneAsync(initialProfile.ToDocument(), cancellationToken: Cts.Token);

        var repository = CreateDevProfileRepository();
        var newRole = RoleTitle.From("Senior Developer");
        var now = DateTimeOffset.UtcNow;

        initialProfile.ChangeRole(newRole, now);

        // Act
        await repository.UpdateAsync(initialProfile, Cts.Token);

        // Assert
        var dbProfile = await GetFreshProfile(initialProfile.Id.Value);
        Assert.NotNull(dbProfile);
        Assert.Equal(newRole, dbProfile.Role);
        Assert.Equal(now.ToUnixTimeSeconds(), dbProfile.UpdatedAt.ToUnixTimeSeconds());
    }

    /// <summary>
    /// Purpose: Verify deletion removes the stored document.
    /// When: Deleting an existing profile.
    /// Should: Return true and result in null on subsequent reads.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldRemoveProfile()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile();
        await GetCollection().InsertOneAsync(profile.ToDocument(), cancellationToken: Cts.Token);

        var repository = CreateDevProfileRepository();

        // Act
        var result = await repository.DeleteAsync(profile.Id.Value, Cts.Token);

        // Assert
        Assert.True(result);
        var dbProfile = await GetFreshProfile(profile.Id.Value);
        Assert.Null(dbProfile);
    }

    /// <summary>
    /// Purpose: Verify deleting a non-existent profile returns false.
    /// When: Profile with the given id does not exist.
    /// Should: Return false.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenProfileDoesNotExist()
    {
        // Arrange
        var repository = CreateDevProfileRepository();

        // Act
        var result = await repository.DeleteAsync(Guid.NewGuid(), Cts.Token);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Purpose: Verify retrieving all profiles returns all stored documents.
    /// When: Multiple documents exist in collection.
    /// Should: Return at least the seeded amount of domain profiles.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ShouldReturnProfiles()
    {
        // Arrange
        var profiles = DataGenerator.CreateDeveloperProfiles(2);
        await GetCollection().InsertManyAsync(profiles.Select(profile => profile.ToDocument()), cancellationToken: Cts.Token);

        var repository = CreateDevProfileRepository();

        // Act
        var result = await repository.GetAllAsync(Cts.Token);

        // Assert
        Assert.True(result.Count >= 2);
    }

    private IMongoCollection<DeveloperProfileDocument> GetCollection()
        => MongoClient.GetDatabase(MongoOptions.DatabaseName).GetCollection<DeveloperProfileDocument>("developer_profiles");

    private async Task<DeveloperProfile?> GetFreshProfile(Guid id)
    {
        var document = await GetCollection()
            .Find(profileDocument => profileDocument.Id == id)
            .FirstOrDefaultAsync(Cts.Token);

        return document?.ToDomain();
    }
}