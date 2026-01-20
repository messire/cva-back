using CVA.Application.Contracts;
using CVA.Application.ProfileService;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="UpdateWorkExperienceHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class UpdateWorkExperienceHandlerTests(PostgresFixture fixture) : ProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that an existing work experience entry can be updated.
    /// When: UpdateWorkExperienceCommand is handled for an existing entry.
    /// Should: Update the entry in the database.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldUpdateWorkExperience()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        profile.AddWorkExperience(
            CompanyName.From("Old Co"),
            null,
            RoleTitle.From("Old Role"),
            null,
            DateRange.From(new DateOnly(2010, 1, 1), null),
            [],
            DateTimeOffset.UtcNow);
        await SeedProfileAsync(profile, Cts.Token);
        var workId = profile.WorkExperience.First().Id.Value;

        var request = new UpsertWorkExperienceRequest
        {
            Company = "Updated Co",
            Role = "Updated Role",
            StartDate = new DateOnly(2015, 1, 1)
        };
        var command = new UpdateWorkExperienceCommand(workId, request);
        var handler = new UpdateWorkExperienceHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        var updatedWork = result.Value?.WorkExperience.First(dto => dto.Id == workId);
        Assert.Equal(request.Company, updatedWork?.Company);

        var dbProfile = await GetFreshProfileAsync(CurrentUserId, Cts.Token);
        Assert.NotNull(dbProfile);
        var dbWork = dbProfile.WorkExperience.First(item => item.Id.Value == workId);
        Assert.Equal(request.Company, dbWork.Company.Value);
        Assert.Equal(request.Role, dbWork.Role?.Value);
    }

    /// <summary>
    /// Purpose: Verify that updating non-existing work experience fails.
    /// When: UpdateWorkExperienceCommand is handled with an invalid ID.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenWorkExperienceNotFound()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        var request = new UpsertWorkExperienceRequest { Company = "Any" };
        var command = new UpdateWorkExperienceCommand(Guid.NewGuid(), request);
        var handler = new UpdateWorkExperienceHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Work experience not found.", result.Error?.Message);
    }
}
