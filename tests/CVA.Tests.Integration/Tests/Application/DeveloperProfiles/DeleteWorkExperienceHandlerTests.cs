using CVA.Application.ProfileService;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="DeleteWorkExperienceHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class DeleteWorkExperienceHandlerTests(PostgresFixture fixture) : ProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that a work experience entry can be successfully deleted.
    /// When: DeleteWorkExperienceCommand is handled for an existing entry.
    /// Should: Remove the entry from the database.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldDeleteWorkExperience()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        profile.AddWorkExperience(
            CompanyName.From("To Delete"),
            null,
            RoleTitle.From("Dev"),
            null,
            DateRange.From(new DateOnly(2020, 1, 1), null),
            [],
            DateTimeOffset.UtcNow);
        await SeedProfileAsync(profile, Cts.Token);
        var workId = profile.WorkExperience.First().Id.Value;

        var command = new DeleteWorkExperienceCommand(workId);
        var handler = new DeleteWorkExperienceHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(result.Value?.WorkExperience!, dto => dto.Id == workId);

        var dbProfile = await GetFreshProfileAsync(CurrentUserId, Cts.Token);
        Assert.NotNull(dbProfile);
        Assert.Empty(dbProfile.WorkExperience);
    }

    /// <summary>
    /// Purpose: Verify that deleting work experience from a non-existing profile fails.
    /// When: DeleteWorkExperienceCommand is handled for a user without a profile.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProfileNotFound()
    {
        // Arrange
        var command = new DeleteWorkExperienceCommand(Guid.NewGuid());
        var handler = new DeleteWorkExperienceHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Profile not found.", result.Error?.Message);
    }

    /// <summary>
    /// Purpose: Verify that deleting non-existing work experience fails.
    /// When: DeleteWorkExperienceCommand is handled with an invalid ID.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenWorkExperienceNotFound()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        var command = new DeleteWorkExperienceCommand(Guid.NewGuid());
        var handler = new DeleteWorkExperienceHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Work experience not found.", result.Error?.Message);
    }
}
