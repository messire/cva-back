using CVA.Application.Contracts;
using CVA.Application.ProfileService;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="ReplaceProfileHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class ReplaceProfileHandlerTests(PostgresFixture fixture) : DeveloperProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that a profile can be fully replaced.
    /// When: ReplaceProfileCommand is handled for an existing profile.
    /// Should: Update all profile fields and related collections in the database.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReplaceProfile()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        var request = new ReplaceProfileRequest
        {
            FirstName = "NewFirstName",
            LastName = "NewLastName",
            Role = "New Role",
            Summary = "New Summary",
            Email = "new@example.com",
            Website = "https://newsite.com",
            Location = new LocationDto { City = "NewCity", Country = "NewCountry" },
            SocialLinks = new SocialLinksDto { GitHub = "https://github.com/new" },
            Skills = ["C#", "SQL"],
            Projects = [new ProjectDto { Name = "New P", LinkUrl = "https://newp.com" }],
            WorkExperience = [new WorkExperienceDto { Company = "New C", Role = "Dev", StartDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)) }]
        };
        var command = new ReplaceProfileCommand(request);
        var handler = new ReplaceProfileHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(request.Role, result.Value?.Role);

        var dbProfile = await GetFreshProfileAsync(CurrentUserId, Cts.Token);
        Assert.NotNull(dbProfile);
        Assert.Equal(request.FirstName, dbProfile.Name.FirstName);
        Assert.Equal(request.LastName, dbProfile.Name.LastName);
        Assert.Equal(request.Email, dbProfile.Contact.Email.Value);
        Assert.Equal(request.Skills, dbProfile.Skills.Select(tag => tag.Value));
        Assert.Single(dbProfile.Projects);
        Assert.Single(dbProfile.WorkExperience);
    }

    /// <summary>
    /// Purpose: Verify that replacing a non-existing profile fails.
    /// When: ReplaceProfileCommand is handled for a user without a profile.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProfileNotFound()
    {
        // Arrange
        var request = new ReplaceProfileRequest { FirstName = "Any", LastName = "Any", Email = "any@test.com" };
        var command = new ReplaceProfileCommand(request);
        var handler = new ReplaceProfileHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("fail", result.Error?.Message.ToLowerInvariant());
    }
}
