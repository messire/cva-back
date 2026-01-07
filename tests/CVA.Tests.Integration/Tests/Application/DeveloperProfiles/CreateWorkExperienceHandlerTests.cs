using CVA.Application.Contracts;
using CVA.Application.DeveloperProfiles;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="CreateWorkExperienceHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class CreateWorkExperienceHandlerTests(PostgresFixture fixture) : DeveloperProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that work experience can be successfully added to a profile.
    /// When: CreateWorkExperienceCommand is handled for an existing profile.
    /// Should: Add the work experience entry to the database.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldAddWorkExperience()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        var request = new UpsertWorkExperienceRequest
        {
            Company = "Test Company",
            Role = "Senior Dev",
            Description = "Test Job Description",
            StartDate = new DateOnly(2020, 1, 1),
            EndDate = new DateOnly(2022, 1, 1),
            TechStack = ["C#", "SQL"],
            Location = new LocationDto { City = "City", Country = "Country" }
        };
        var command = new CreateWorkExperienceCommand(request);
        var handler = new CreateWorkExperienceHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(result.Value?.WorkExperience!, dto => dto.Company == request.Company);

        var dbProfile = await GetFreshProfileAsync(CurrentUserId, Cts.Token);
        Assert.NotNull(dbProfile);
        Assert.Single(dbProfile.WorkExperience);
        var dbWork = dbProfile.WorkExperience[0];
        Assert.Equal(request.Company, dbWork.Company.Value);
        Assert.Equal(request.Role, dbWork.Role?.Value);
        Assert.Equal(request.StartDate, dbWork.Period.Start);
    }

    /// <summary>
    /// Purpose: Verify that adding work experience fails if the profile does not exist.
    /// When: CreateWorkExperienceCommand is handled for a non-existing profile.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProfileNotFound()
    {
        // Arrange
        var request = new UpsertWorkExperienceRequest { Company = "Any" };
        var command = new CreateWorkExperienceCommand(request);
        var handler = new CreateWorkExperienceHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Profile not found.", result.Error?.Message);
    }
}
