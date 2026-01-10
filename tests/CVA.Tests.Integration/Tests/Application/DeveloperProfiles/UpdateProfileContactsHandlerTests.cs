using CVA.Application.Contracts;
using CVA.Application.ProfileService;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Integration tests for the <see cref="UpdateProfileContactsHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class UpdateProfileContactsHandlerTests(PostgresFixture fixture) : DeveloperProfileHandlerTestBase(fixture)
{
    /// <summary>
    /// Purpose: Verify that contact information can be updated.
    /// When: UpdateProfileContactsCommand is handled for an existing profile.
    /// Should: Update email, website, location, and social links in the database.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldUpdateContacts()
    {
        // Arrange
        var profile = DataGenerator.CreateDeveloperProfile(CurrentUserId);
        await SeedProfileAsync(profile, Cts.Token);

        var request = new UpdateProfileContactsRequest
        {
            Email = "updated@example.com",
            Website = "https://updated.com",
            Location = new LocationDto { City = "UpdatedCity", Country = "UpdatedCountry" },
            SocialLinks = new SocialLinksDto { LinkedIn = "https://linkedin.com/updated" }
        };
        var command = new UpdateProfileContactsCommand(request);
        var handler = new UpdateProfileContactsHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(request.Email, result.Value?.Location?.City == "UpdatedCity" ? request.Email : result.Value?.SocialLinks?.LinkedIn == "https://linkedin.com/updated" ? request.Email : "fail"); // Simple check

        var dbProfile = await GetFreshProfileAsync(CurrentUserId, Cts.Token);
        Assert.NotNull(dbProfile);
        Assert.Equal(request.Email, dbProfile.Contact.Email.Value);
        Assert.Equal(request.Website, dbProfile.Contact.Website?.Value);
        Assert.Equal(request.Location.City, dbProfile.Contact.Location?.City);
        Assert.Equal(request.SocialLinks.LinkedIn, dbProfile.Social.LinkedIn?.Value);
    }

    /// <summary>
    /// Purpose: Verify that updating contacts fails if the profile does not exist.
    /// When: UpdateProfileContactsCommand is handled for a non-existing profile.
    /// Should: Return a failed result.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldFail_WhenProfileNotFound()
    {
        // Arrange
        var request = new UpdateProfileContactsRequest { Email = "any@test.com" };
        var command = new UpdateProfileContactsCommand(request);
        var handler = new UpdateProfileContactsHandler(CreateRepository(), UserAccessorMock.Object);

        // Act
        var result = await handler.HandleAsync(command, Cts.Token);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Profile not found.", result.Error?.Message);
    }
}
