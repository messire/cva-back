using AutoFixture;
using CVA.Application.ProfileService;
using CVA.Domain.Models;

namespace CVA.Tests.Unit.Application.Mapping;

/// <summary>
/// Unit tests for developer profile DTO mapping.
/// </summary>
[Trait(Layer.Application, Category.Mapping)]
public sealed class DeveloperProfileMappingTests
{
    private readonly IFixture _fixture;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeveloperProfileMappingTests"/> class.
    /// </summary>
    public DeveloperProfileMappingTests()
    {
        _fixture = new Fixture().Customize(new ApplicationTestCustomization());
    }

    /// <summary>
    /// Purpose: Verify that all profile fields are mapped to the DTO.
    /// When: ToDto is called on a populated <see cref="DeveloperProfile"/>.
    /// Should: Copy identifiers, primitives, and nested collections.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapProfileToDto()
    {
        // Arrange
        var profile = _fixture.Create<DeveloperProfile>();

        // Act
        var dto = profile.ToDto();

        // Assert
        Assert.Equal(profile.Id.Value, dto.Id);
        Assert.Equal(profile.Name.FirstName, dto.FirstName);
        Assert.Equal(profile.Name.LastName, dto.LastName);
        Assert.Equal(profile.Role?.Value, dto.Role);
        Assert.Equal(profile.Summary?.Value, dto.Summary);
        Assert.Equal(profile.Avatar?.ImageUrl.Value, dto.AvatarUrl);
        Assert.Equal(profile.Contact.Email.Value, dto.Email);
        Assert.Equal(profile.Contact.Phone?.Value, dto.Phone);
        Assert.Equal(profile.Contact.Website?.Value, dto.Website);
        
        Assert.Equal(profile.Social.LinkedIn?.Value, dto.SocialLinks?.LinkedIn);
        Assert.Equal(profile.Social.GitHub?.Value, dto.SocialLinks?.GitHub);
        
        Assert.Equal(profile.Projects.Count, dto.Projects.Length);
        Assert.Equal(profile.WorkExperience.Count, dto.WorkExperience.Length);
    }
}
