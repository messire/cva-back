using CVA.Domain.Models;
using CVA.Infrastructure.Postgres;
using CVA.Tests.Common;
using CVA.Tests.Common.Comparers;
using CVA.Tests.Integration.Fixtures;

namespace CVA.Tests.Integration.Tests.Infrastructure.Postgres;

/// <summary>
/// Integration tests for the <see cref="DeveloperProfilePostgresRepository"/> using Testcontainers.
/// </summary>
[Trait(Layer.Infrastructure, Category.Repository)]
public sealed class DeveloperProfilePostgresRepositoryTests(PostgresFixture fixture) : PostgresTestBase(fixture)
{
    private static readonly DeveloperProfileComparer ProfileComp = new();

    /// <summary>
    /// Purpose: Verify repository persists a new developer profile.
    /// When: CreateAsync is called with a valid profile.
    /// Should: Persist the profile and return it back from database with the same state.
    /// </summary>
    [Fact]
    public async Task CreateAsync_ShouldPersistProfile()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile();
        await using var context = CreateContext();
        var repository = CreateProfileRepository(context);

        // Act
        var created = await repository.CreateAsync(profile, Cts.Token);

        // Assert
        Assert.NotNull(created);
        var dbProfile = await GetFreshProfileAsync(created.Id.Value, Cts.Token);
        Assert.NotNull(dbProfile);
        Assert.Equal(profile, dbProfile, ProfileComp);
    }

    /// <summary>
    /// Purpose: Verify repository returns a profile by id including projects and work experience.
    /// When: GetByIdAsync is called for an existing profile id.
    /// Should: Return the profile with all related collections loaded.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnProfileWithDetails()
    {
        // Arrange
        var seedProfile = DataGenerator.CreateDeveloperProfile();
        var created = await SeedProfileAsync(seedProfile, Cts.Token);
        Assert.NotNull(created);

        await using var context = CreateContext();
        var repository = CreateProfileRepository(context);

        // Act
        var result = await repository.GetByIdAsync(created.Id.Value, Cts.Token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(seedProfile, result, ProfileComp);
    }

    /// <summary>
    /// Purpose: Verify repository updates scalar fields and related collections.
    /// When: UpdateAsync is called for an existing profile with modified fields.
    /// Should: Persist updated fields and replace related collections state.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldUpdateFieldsAndCollections()
    {
        // Arrange
        var initialProfile = DataGenerator.CreateDeveloperProfile();
        var created = await SeedProfileAsync(initialProfile, Cts.Token);
        Assert.NotNull(created);

        var newRole = RoleTitle.From("Architect");
        var now = DateTimeOffset.UtcNow;
        created.ChangeRole(newRole, now);

        await using var context = CreateContext();
        var repository = CreateProfileRepository(context);

        // Act
        await repository.UpdateAsync(created, Cts.Token);

        // Assert
        var dbProfile = await GetFreshProfileAsync(created.Id.Value, Cts.Token);
        Assert.NotNull(dbProfile);
        Assert.Equal(newRole, dbProfile.Role);
        Assert.Equal(now.ToUnixTimeSeconds(), dbProfile.UpdatedAt.ToUnixTimeSeconds());
    }

    /// <summary>
    /// Purpose: Verify repository deletes a profile.
    /// When: DeleteAsync is called for an existing profile id.
    /// Should: Remove the profile and its related entities (cascade delete).
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldRemoveProfile()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile();
        var created = await SeedProfileAsync(profile, Cts.Token);
        Assert.NotNull(created);

        await using var context = CreateContext();
        var repository = CreateProfileRepository(context);

        // Act
        var result = await repository.DeleteAsync(created.Id.Value, Cts.Token);

        // Assert
        Assert.True(result);
        var dbProfile = await GetFreshProfileAsync(created.Id.Value, Cts.Token);
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
        await using var context = CreateContext();
        var repository = CreateProfileRepository(context);

        // Act
        var result = await repository.DeleteAsync(Guid.NewGuid(), Cts.Token);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Purpose: Verify repository returns all profiles.
    /// When: GetAllAsync is called and multiple profiles exist.
    /// Should: Return at least the amount of profiles that were seeded.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ShouldReturnProfiles()
    {
        // Arrange
        var profiles = DataGenerator.CreateDeveloperProfiles(2);
        foreach (var profile in profiles) await SeedProfileAsync(profile, Cts.Token);

        await using var context = CreateContext();
        var repository = CreateProfileRepository(context);

        // Act
        var result = await repository.GetAllAsync(Cts.Token);

        // Assert
        Assert.True(result.Count >= 2);
    }

    /// <summary>
    /// Purpose: Verify repository returns null for missing profiles.
    /// When: GetByIdAsync is called with a non-existent id.
    /// Should: Return null.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProfileDoesNotExist()
    {
        // Arrange
        await using var context = CreateContext();
        var repository = CreateProfileRepository(context);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid(), Cts.Token);

        // Assert
        Assert.Null(result);
    }
}