using CVA.Application.DeveloperProfiles;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="DeleteProjectHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class DeleteProjectHandlerTests(PostgresFixture fixture) : DeveloperProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that a project can be successfully deleted from a profile.
    /// When: DeleteProjectCommand is handled for an existing project.
    /// Should: Remove the project from the database and return the updated profile.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldDeleteProject()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        profile.AddProject(
            ProjectName.From("To Delete"),
            ProjectDescription.From("Desc"),
            null,
            ProjectLink.From("https://test.com"),
            [],
            DateTimeOffset.UtcNow);
        await SeedProfileAsync(profile, Cts.Token);
        var projectId = profile.Projects.First().Id.Value;

        var command = new DeleteProjectCommand(projectId);
        var handler = new DeleteProjectHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(result.Value?.Projects!, dto => dto.Id == projectId);

        var dbProfile = await GetFreshProfileAsync(CurrentUserId, Cts.Token);
        Assert.NotNull(dbProfile);
        Assert.Empty(dbProfile.Projects);
    }

    /// <summary>
    /// Purpose: Verify that deleting a project from a non-existing profile fails.
    /// When: DeleteProjectCommand is handled for a user without a profile.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProfileNotFound()
    {
        // Arrange
        var command = new DeleteProjectCommand(Guid.NewGuid());
        var handler = new DeleteProjectHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Profile not found.", result.Error?.Message);
    }

    /// <summary>
    /// Purpose: Verify that deleting a non-existing project fails.
    /// When: DeleteProjectCommand is handled with an invalid project ID.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProjectNotFound()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        var command = new DeleteProjectCommand(Guid.NewGuid());
        var handler = new DeleteProjectHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Project not found.", result.Error?.Message);
    }
}
