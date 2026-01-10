using CVA.Application.ProfileService;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="GetMyDeveloperProfileHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class GetMyDeveloperProfileHandlerTests(PostgresFixture fixture) : DeveloperProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that the current user's profile can be retrieved.
    /// When: GetMyDeveloperProfileQuery is handled for an existing profile.
    /// Should: Return the correct profile DTO.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnCurrentProfile()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        var query = new GetMyDeveloperProfileQuery();
        var handler = new GetMyDeveloperProfileHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(CurrentUserId, result.Value?.Id);
    }

    /// <summary>
    /// Purpose: Verify that retrieving a profile fails if it does not exist.
    /// When: GetMyDeveloperProfileQuery is handled for a user without a profile.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProfileNotFound()
    {
        // Arrange
        var query = new GetMyDeveloperProfileQuery();
        var handler = new GetMyDeveloperProfileHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(query, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("not found", result.Error?.Message);
    }
}
