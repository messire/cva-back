using CVA.Application.DeveloperProfiles;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="GetDeveloperProfileByIdHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class GetDeveloperProfileByIdHandlerTests(PostgresFixture fixture) : DeveloperProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that a profile can be retrieved by its ID.
    /// When: GetDeveloperProfileByIdQuery is handled for an existing profile.
    /// Should: Return the correct profile DTO.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnProfile()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var profile = DataGenerator.CreateDeveloperProfile(profileId);
        await SeedProfileAsync(profile, Cts.Token);

        var query = new GetDeveloperProfileByIdQuery(profileId);
        var handler = new GetDeveloperProfileByIdHandler(CreateRepository());

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(profileId, result.Value?.Id);
    }

    /// <summary>
    /// Purpose: Verify that retrieving a profile by an invalid ID fails.
    /// When: GetDeveloperProfileByIdQuery is handled for a non-existing ID.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProfileNotFound()
    {
        // Arrange
        var query = new GetDeveloperProfileByIdQuery(Guid.NewGuid());
        var handler = new GetDeveloperProfileByIdHandler(CreateRepository());

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("not found", result.Error?.Message);
    }
}
