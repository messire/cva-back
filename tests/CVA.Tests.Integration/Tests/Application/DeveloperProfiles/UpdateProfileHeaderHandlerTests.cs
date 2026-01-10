using CVA.Application.Contracts;
using CVA.Application.ProfileService;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="UpdateProfileHeaderHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class UpdateProfileHeaderHandlerTests(PostgresFixture fixture) : DeveloperProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that profile header information can be updated.
    /// When: UpdateProfileHeaderCommand is handled for an existing profile.
    /// Should: Update name, role, avatar, and experience in the database.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldUpdateHeader()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        var request = new UpdateProfileHeaderRequest
        {
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            Role = "Updated Role",
            AvatarUrl = "https://updated.com/avatar.png",
            YearsOfExperience = 10
        };
        var command = new UpdateProfileHeaderCommand(request);
        var handler = new UpdateProfileHeaderHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(request.Role, result.Value?.Role);

        var dbProfile = await GetFreshProfileAsync(CurrentUserId, Cts.Token);
        Assert.NotNull(dbProfile);
        Assert.Equal(request.FirstName, dbProfile.Name.FirstName);
        Assert.Equal(request.LastName, dbProfile.Name.LastName);
        Assert.Equal(request.Role, dbProfile.Role?.Value);
        Assert.Equal(request.AvatarUrl, dbProfile.Avatar?.ImageUrl.Value);
        Assert.Equal(request.YearsOfExperience, dbProfile.YearsOfExperience.Value);
    }

    /// <summary>
    /// Purpose: Verify that updating header fails if the profile does not exist.
    /// When: UpdateProfileHeaderCommand is handled for a non-existing profile.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProfileNotFound()
    {
        // Arrange
        var request = new UpdateProfileHeaderRequest { FirstName = "Any" };
        var command = new UpdateProfileHeaderCommand(request);
        var handler = new UpdateProfileHeaderHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Profile not found.", result.Error?.Message);
    }
}
