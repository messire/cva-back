using CVA.Application.DeveloperProfiles;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="GetDeveloperProfilesCatalogHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class GetDeveloperProfilesCatalogHandlerTests(PostgresFixture fixture) : DeveloperProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that developer profiles can be retrieved with filtering.
    /// When: GetDeveloperProfilesCatalogQuery is handled with search and skill filters.
    /// Should: Return only the profiles matching the criteria.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnFilteredProfiles()
    {
        // Arrange
        var profile1 = DataGenerator.CreateDeveloperProfile();
        profile1.ChangeName(PersonName.From("John", "Doe"), DateTimeOffset.UtcNow);
        profile1.ChangeRole(RoleTitle.From("Backend"), DateTimeOffset.UtcNow);
        profile1.ReplaceSkills([SkillTag.From("C#"), SkillTag.From("Postgres")], DateTimeOffset.UtcNow);

        var profile2 = DataGenerator.CreateDeveloperProfile();
        profile2.ChangeName(PersonName.From("Jane", "Smith"), DateTimeOffset.UtcNow);
        profile2.ChangeRole(RoleTitle.From("Frontend"), DateTimeOffset.UtcNow);
        profile2.ReplaceSkills([SkillTag.From("React")], DateTimeOffset.UtcNow);

        await SeedProfileAsync(profile1, Cts.Token);
        await SeedProfileAsync(profile2, Cts.Token);

        var query = new GetDeveloperProfilesCatalogQuery("John", ["C#"], null, null);
        var handler = new GetDeveloperProfilesCatalogHandler(CreateRepository());

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value!);
        Assert.Equal("John", result.Value?[0].FirstName);
    }

    /// <summary>
    /// Purpose: Verify that the catalog returns an empty list if no profiles match.
    /// When: GetDeveloperProfilesCatalogQuery is handled with criteria matching no profiles.
    /// Should: Return an empty list.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnEmptyList_WhenNoProfilesMatch()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile();
        await SeedProfileAsync(profile, Cts.Token);

        var query = new GetDeveloperProfilesCatalogQuery("NonExistingName", null!, null, null);
        var handler = new GetDeveloperProfilesCatalogHandler(CreateRepository());

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value!);
    }
}
