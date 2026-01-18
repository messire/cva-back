using CVA.Application.Contracts;
using CVA.Application.ProfileService;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="CreateProjectHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class CreateProjectHandlerTests(PostgresFixture fixture) : ProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that a project can be successfully added to an existing profile.
    /// When: CreateProjectCommand is handled for an existing profile.
    /// Should: Add the project to the database and return the updated profile.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldAddProjectToProfile()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        var request = new UpsertProjectRequest
        {
            Name = "New Project",
            Description = "Test Description",
            LinkUrl = "https://github.com/test/project",
            IconUrl = "https://example.com/icon.png",
            TechStack = ["C#", "Postgres"]
        };
        var command = new CreateProjectCommand(request);
        var handler = new CreateProjectHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Contains(result.Value.Projects, dto => dto.Name == request.Name);

        var dbProfile = await GetFreshProfileAsync(CurrentUserId, Cts.Token);
        Assert.NotNull(dbProfile);
        Assert.Single(dbProfile.Projects);
        var dbProject = dbProfile.Projects[0];
        Assert.Equal(request.Name, dbProject.Name.Value);
        Assert.Equal(request.Description, dbProject.Description?.Value);
        Assert.Equal(request.LinkUrl, dbProject.Link.Value?.Value);
        Assert.Equal(request.IconUrl, dbProject.Icon?.ImageUrl.Value);
        Assert.Equal(request.TechStack, dbProject.TechStack.Select(tag => tag.Value));
    }

    /// <summary>
    /// Purpose: Verify that adding a project fails if the profile does not exist.
    /// When: CreateProjectCommand is handled for a non-existing profile.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProfileNotFound()
    {
        // Arrange
        var request = new UpsertProjectRequest
        {
            Name = "New Project",
            LinkUrl = "https://github.com/test/project"
        };
        var command = new CreateProjectCommand(request);
        var handler = new CreateProjectHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Profile not found.", result.Error?.Message);
    }
}
