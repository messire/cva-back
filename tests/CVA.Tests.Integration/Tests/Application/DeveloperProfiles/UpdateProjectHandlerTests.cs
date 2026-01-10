using CVA.Application.Contracts;
using CVA.Application.ProfileService;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="UpdateProjectHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class UpdateProjectHandlerTests(PostgresFixture fixture) : DeveloperProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that an existing project can be updated.
    /// When: UpdateProjectCommand is handled for an existing project.
    /// Should: Update the project in the database and return the updated profile.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldUpdateProject()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        profile.AddProject(
            ProjectName.From("Old Name"),
            ProjectDescription.From("Old Description"),
            ProjectIcon.TryFrom("https://old.com/icon.png"),
            ProjectLink.From("https://old.com"),
            [TechTag.From("Old Tech")],
            DateTimeOffset.UtcNow);
        await SeedProfileAsync(profile, Cts.Token);
        var projectId = profile.Projects.First().Id.Value;

        var request = new UpsertProjectRequest
        {
            Name = "Updated Project",
            Description = "Updated Description",
            LinkUrl = "https://github.com/test/updated",
            IconUrl = "https://example.com/updated.png",
            TechStack = ["C#", "Postgres"]
        };
        var command = new UpdateProjectCommand(projectId, request);
        var handler = new UpdateProjectHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        var updatedProject = result.Value?.Projects.First(dto => dto.Id == projectId);
        Assert.Equal(request.Name, updatedProject?.Name);

        var dbProfile = await GetFreshProfileAsync(CurrentUserId, Cts.Token);
        Assert.NotNull(dbProfile);
        var dbProject = dbProfile.Projects.First(item => item.Id.Value == projectId);
        Assert.Equal(request.Name, dbProject.Name.Value);
        Assert.Equal(request.Description, dbProject.Description?.Value);
        Assert.Equal(request.LinkUrl, dbProject.Link.Value?.Value);
        Assert.Equal(request.IconUrl, dbProject.Icon?.ImageUrl.Value);
        Assert.Equal(request.TechStack, dbProject.TechStack.Select(tag => tag.Value));
    }

    /// <summary>
    /// Purpose: Verify that updating a non-existing project fails.
    /// When: UpdateProjectCommand is handled with an invalid project ID.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProjectNotFound()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        var request = new UpsertProjectRequest { Name = "Updated", LinkUrl = "https://test.com" };
        var command = new UpdateProjectCommand(Guid.NewGuid(), request);
        var handler = new UpdateProjectHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Project not found.", result.Error?.Message);
    }
}
