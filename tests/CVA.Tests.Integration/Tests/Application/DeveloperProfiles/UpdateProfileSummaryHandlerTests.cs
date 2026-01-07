using CVA.Application.Contracts;
using CVA.Application.DeveloperProfiles;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="UpdateProfileSummaryHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class UpdateProfileSummaryHandlerTests(PostgresFixture fixture) : DeveloperProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that profile summary can be updated.
    /// When: UpdateProfileSummaryCommand is handled for an existing profile.
    /// Should: Update the summary in the database.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldUpdateSummary()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        var request = new UpdateProfileSummaryRequest { Summary = "Updated Summary" };
        var command = new UpdateProfileSummaryCommand(request);
        var handler = new UpdateProfileSummaryHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(request.Summary, result.Value?.Summary);

        var dbProfile = await GetFreshProfileAsync(CurrentUserId, Cts.Token);
        Assert.NotNull(dbProfile);
        Assert.Equal(request.Summary, dbProfile.Summary?.Value);
    }

    /// <summary>
    /// Purpose: Verify that updating summary fails if the profile does not exist.
    /// When: UpdateProfileSummaryCommand is handled for a non-existing profile.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProfileNotFound()
    {
        // Arrange
        var request = new UpdateProfileSummaryRequest { Summary = "Any" };
        var command = new UpdateProfileSummaryCommand(request);
        var handler = new UpdateProfileSummaryHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Profile not found.", result.Error?.Message);
    }
}
