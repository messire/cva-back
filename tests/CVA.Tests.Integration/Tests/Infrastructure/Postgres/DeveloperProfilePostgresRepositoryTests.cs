using CVA.Infrastructure.Postgres;
using CVA.Tests.Common.Comparers;

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

    /// <summary>
    /// Purpose: Verify repository correctly synchronizes nested collections (Projects, WorkExperience).
    /// When: UpdateAsync is called with modified collections.
    /// Should: Update existing items, add new ones, and remove missing ones without EF tracking issues.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldSyncCollectionsCorrectly()
    {
        // Arrange
        var initialProfile = DataGenerator.CreateDeveloperProfile();
        var created = await SeedProfileAsync(initialProfile, Cts.Token);
        Assert.NotNull(created);
        Assert.NotEmpty(created.Projects);
        Assert.NotEmpty(created.WorkExperience);

        var projectToUpdate = created.Projects[0];
        var workToUpdate = created.WorkExperience[0];

        var updatedProjects = new List<ProjectItem>
        {
            ProjectItem.FromPersistence(
                projectToUpdate.Id,
                ProjectName.From("Updated Project"),
                projectToUpdate.Description,
                projectToUpdate.Icon,
                projectToUpdate.Link,
                projectToUpdate.TechStack),
            ProjectItem.FromPersistence(
                new ProjectId(Guid.NewGuid()),
                ProjectName.From("New Project"),
                null, null, ProjectLink.From(Url.TryFrom("https://new.com")?.Value), [])
        };

        var updatedWork = new List<WorkExperienceItem>
        {
            WorkExperienceItem.FromPersistence(
                workToUpdate.Id,
                CompanyName.From("Updated Company"),
                workToUpdate.Location,
                workToUpdate.Role,
                workToUpdate.Description,
                workToUpdate.Period,
                workToUpdate.TechStack),
            WorkExperienceItem.FromPersistence(
                new WorkExperienceId(Guid.NewGuid()),
                CompanyName.From("New Company"),
                null, RoleTitle.From("New Role"), null,
                DateRange.From(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)), null), [])
        };

        var updatedProfile = DeveloperProfile.FromPersistence(
            created.Id, created.Name, created.Role, created.Summary, created.Avatar,
            created.Contact, created.Social, created.Verification, created.OpenToWork,
            created.Skills, updatedProjects, updatedWork,
            created.CreatedAt, DateTimeOffset.UtcNow);

        await using var context = CreateContext();
        var repository = CreateProfileRepository(context);

        // Act
        await repository.UpdateAsync(updatedProfile, Cts.Token);

        // Assert
        var dbProfile = await GetFreshProfileAsync(created.Id.Value, Cts.Token);
        Assert.NotNull(dbProfile);
        
        Assert.Equal(2, dbProfile.Projects.Count);
        Assert.Contains(dbProfile.Projects, item => item.Name.Value == "Updated Project" && item.Id == projectToUpdate.Id);
        Assert.Contains(dbProfile.Projects, item => item.Name.Value == "New Project");

        Assert.Equal(2, dbProfile.WorkExperience.Count);
        Assert.Contains(dbProfile.WorkExperience, item => item.Company.Value == "Updated Company" && item.Id == workToUpdate.Id);
        Assert.Contains(dbProfile.WorkExperience, item => item.Company.Value == "New Company");
    }
}