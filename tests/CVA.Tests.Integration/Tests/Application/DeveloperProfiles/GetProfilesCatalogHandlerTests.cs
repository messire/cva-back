using CVA.Application.ProfileService;
using CVA.Domain.Interfaces;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="GetProfilesCatalogHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class GetProfilesCatalogHandlerTests(PostgresFixture fixture) : ProfileHandlerTestBase(fixture)
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

        var query = new GetProfilesCatalogQuery("John", ["C#"], null, null, 1, 10, null, null);
        var handler = new GetProfilesCatalogHandler(CreateRepository());

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value.Items);
        Assert.Equal("John", result.Value.Items[0].FirstName);
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

        var query = new GetProfilesCatalogQuery("NonExistingName", [], null, null, 1, 10, null, null);
        var handler = new GetProfilesCatalogHandler(CreateRepository());

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value.Items);
    }

    /// <summary>
    /// Purpose: Verify that pagination works correctly.
    /// When: GetProfilesCatalogQuery is handled with specific page and pageSize.
    /// Should: Return correct subset of profiles and pagination info.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnCorrectPage()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
        {
            var profile = DataGenerator.CreateDeveloperProfile();
            profile.ChangeName(PersonName.From($"First{i}", $"Last{i}"), DateTimeOffset.UtcNow);
            await SeedProfileAsync(profile, Cts.Token);
        }

        var pageSize = 2;
        var pageNumber = 2;
        var query = new GetProfilesCatalogQuery(null, [], null, null, pageNumber, pageSize, ProfilesSortFields.Name, SortOrders.Asc);
        var handler = new GetProfilesCatalogHandler(CreateRepository());

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(pageSize, result.Value.Items.Length);
        Assert.Equal(pageNumber, result.Value.Pagination.Number);
        Assert.Equal(pageSize, result.Value.Pagination.Size);
        Assert.Equal(5, result.Value.Pagination.TotalCount);
        Assert.Equal(3, result.Value.Pagination.TotalPages);
        Assert.Equal("First3", result.Value.Items[0].FirstName);
    }

    /// <summary>
    /// Purpose: Verify that sorting by name works.
    /// When: GetProfilesCatalogQuery is handled with SortField Name.
    /// Should: Return profiles sorted by name.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnSortedProfiles()
    {
        // Arrange
        var p1 = DataGenerator.CreateDeveloperProfile();
        p1.ChangeName(PersonName.From("Alice", "A"), DateTimeOffset.UtcNow);
        var p2 = DataGenerator.CreateDeveloperProfile();
        p2.ChangeName(PersonName.From("Bob", "B"), DateTimeOffset.UtcNow);
        
        await SeedProfileAsync(p1, Cts.Token);
        await SeedProfileAsync(p2, Cts.Token);

        var query = new GetProfilesCatalogQuery(null, [], null, null, 1, 10, ProfilesSortFields.Name, SortOrders.Desc);
        var handler = new GetProfilesCatalogHandler(CreateRepository());

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Bob", result.Value.Items[0].FirstName);
        Assert.Equal("Alice", result.Value.Items[1].FirstName);
    }

    /// <summary>
    /// Purpose: Verify filtering by OpenToWork status.
    /// When: GetProfilesCatalogQuery is handled with OpenToWork filter.
    /// Should: Return only profiles with matching status.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFilterByOpenToWork()
    {
        // Arrange
        var p1 = DataGenerator.CreateDeveloperProfile();
        p1.SetOpenToWork(true, DateTimeOffset.UtcNow);
        var p2 = DataGenerator.CreateDeveloperProfile();
        p2.SetOpenToWork(false, DateTimeOffset.UtcNow);

        await SeedProfileAsync(p1, Cts.Token);
        await SeedProfileAsync(p2, Cts.Token);

        var query = new GetProfilesCatalogQuery(null, [], true, null, 1, 10, null, null);
        var handler = new GetProfilesCatalogHandler(CreateRepository());

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.All(result.Value.Items, i => Assert.True(i.OpenToWork));
    }

    /// <summary>
    /// Purpose: Verify filtering by VerificationStatus.
    /// When: GetProfilesCatalogQuery is handled with VerificationStatus filter.
    /// Should: Return only profiles with matching verification level.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFilterByVerificationStatus()
    {
        // Arrange
        var p1 = DataGenerator.CreateDeveloperProfile();
        p1.SetVerified(new VerificationStatus(VerificationLevel.Verified), DateTimeOffset.UtcNow);
        var p2 = DataGenerator.CreateDeveloperProfile();
        // p2 is Unverified by default or we can set it explicitly if needed

        await SeedProfileAsync(p1, Cts.Token);
        await SeedProfileAsync(p2, Cts.Token);

        var query = new GetProfilesCatalogQuery(null, [], null, nameof(VerificationLevel.Verified), 1, 10, null, null);
        var handler = new GetProfilesCatalogHandler(CreateRepository());

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value.Items);
        Assert.Equal(p1.Id.Value, result.Value.Items[0].Id);
    }
}
