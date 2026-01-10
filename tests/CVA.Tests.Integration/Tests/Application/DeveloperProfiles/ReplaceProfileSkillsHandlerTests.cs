using CVA.Application.ProfileService;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="ReplaceProfileSkillsHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class ReplaceProfileSkillsHandlerTests(PostgresFixture fixture) : DeveloperProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that profile skills can be replaced.
    /// When: ReplaceProfileSkillsCommand is handled for an existing profile.
    /// Should: Update the skills list in the database.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReplaceSkills()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        string[] newSkills = ["C#", ".NET", "Postgres"];
        var command = new ReplaceProfileSkillsCommand(newSkills);
        var handler = new ReplaceProfileSkillsHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newSkills, result.Value?.Skills);

        var dbProfile = await GetFreshProfileAsync(CurrentUserId, Cts.Token);
        Assert.NotNull(dbProfile);
        Assert.Equal(newSkills, dbProfile.Skills.Select(tag => tag.Value));
    }

    /// <summary>
    /// Purpose: Verify that replacing skills fails if the profile does not exist.
    /// When: ReplaceProfileSkillsCommand is handled for a non-existing profile.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProfileNotFound()
    {
        // Arrange
        var command = new ReplaceProfileSkillsCommand(["Skill"]);
        var handler = new ReplaceProfileSkillsHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Profile not found.", result.Error?.Message);
    }
}
