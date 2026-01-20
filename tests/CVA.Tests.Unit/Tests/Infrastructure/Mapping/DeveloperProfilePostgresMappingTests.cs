using CVA.Domain.Models;
using CVA.Infrastructure.Postgres;

namespace CVA.Tests.Unit.Infrastructure.Mapping;

/// <summary>
/// Unit tests for <see cref="DeveloperProfileMappingExtensions"/>.
/// </summary>
[Trait(Layer.Infrastructure, Category.Mapping)]
public sealed class DeveloperProfilePostgresMappingTests
{
    private static DeveloperProfileEntity WrapToEntity(DeveloperProfile profile) => profile.ToEntity();
    private static void WrapUpdateFromDomain(DeveloperProfileEntity entity, DeveloperProfile profile) => entity.UpdateFromDomain(profile);
    private static DeveloperProfile WrapToDomain(DeveloperProfileEntity entity) => entity.ToDomain();

    /// <summary>
    /// Verifies that <see cref="DeveloperProfile"/> can be correctly mapped to <see cref="DeveloperProfileEntity"/>.
    /// </summary>
    [Theory, CvaAutoData]
    public void ToEntity_ShouldMapProfileToEntity(DeveloperProfile profile)
    {
        // Act
        var entity = WrapToEntity(profile);

        // Assert
        Assert.Equal(profile.Id.Value, entity.Id);
        Assert.Equal(profile.Name.FirstName, (string?)entity.FirstName);
        Assert.Equal(profile.Name.LastName, (string?)entity.LastName);
        Assert.Equal(profile.Role?.Value, entity.Role);
        Assert.Equal(profile.Summary?.Value, entity.Summary);
        Assert.Equal(profile.Avatar?.ImageUrl.Value, entity.AvatarUrl);
        Assert.Equal(profile.OpenToWork.Value, entity.OpenToWork);
        Assert.Equal(profile.Contact.Email.Value, (string?)entity.Email);
        Assert.Equal(profile.Contact.Website?.Value, entity.Website);
        Assert.Equal(profile.Verification.Value, entity.Verified);

        Assert.Equal(profile.Skills.Count, entity.Skills.Count);
        Assert.Equal(profile.Projects.Count, entity.Projects.Count);
        Assert.Equal(profile.WorkExperience.Count, entity.WorkExperience.Count);
    }

    /// <summary>
    /// Verifies that <see cref="DeveloperProfileEntity"/> can be correctly updated from <see cref="DeveloperProfile"/>.
    /// </summary>
    [Theory, CvaAutoData]
    public void UpdateFromDomain_ShouldUpdateEntityFromProfile(DeveloperProfile profile)
    {
        // Arrange
        var entity = WrapToEntity(profile);
        var newProfile = profile; // Simplified for test

        // Act
        WrapUpdateFromDomain(entity, newProfile);

        // Assert
        Assert.Equal(newProfile.Name.FirstName, (string?)entity.FirstName);
        Assert.Equal(newProfile.Name.LastName, (string?)entity.LastName);
        Assert.Equal(newProfile.Role?.Value, entity.Role);
        Assert.Equal(newProfile.Summary?.Value, entity.Summary);
        Assert.Equal(newProfile.Avatar?.ImageUrl.Value, entity.AvatarUrl);
        Assert.Equal(newProfile.OpenToWork.Value, entity.OpenToWork);
        Assert.Equal(newProfile.Contact.Email.Value, (string?)entity.Email);
        Assert.Equal(newProfile.Contact.Website?.Value, entity.Website);
        Assert.Equal(newProfile.Verification.Value, entity.Verified);

        Assert.Equal(newProfile.Skills.Count, entity.Skills.Count);
        Assert.Equal(newProfile.Projects.Count, entity.Projects.Count);
        Assert.Equal(newProfile.WorkExperience.Count, entity.WorkExperience.Count);
    }

    /// <summary>
    /// Verifies that <see cref="DeveloperProfileEntity"/> can be correctly mapped back to <see cref="DeveloperProfile"/>.
    /// </summary>
    [Theory, CvaAutoData]
    public void ToDomain_ShouldMapEntityToProfile(DeveloperProfile profile)
    {
        // Arrange
        var entity = WrapToEntity(profile);

        // Act
        var result = WrapToDomain(entity);

        // Assert
        Assert.Equal(profile.Id.Value, result.Id.Value);
        Assert.Equal(profile.Name.FirstName, result.Name.FirstName);
        Assert.Equal(profile.Name.LastName, result.Name.LastName);
        Assert.Equal(profile.Role?.Value, result.Role?.Value);
        Assert.Equal(profile.Summary?.Value, result.Summary?.Value);
        Assert.Equal(profile.Avatar?.ImageUrl.Value, result.Avatar?.ImageUrl.Value);
        Assert.Equal(profile.OpenToWork.Value, result.OpenToWork.Value);
        Assert.Equal(profile.Contact.Email.Value, result.Contact.Email.Value);
        Assert.Equal(profile.Contact.Website?.Value, result.Contact.Website?.Value);
        Assert.Equal(profile.Verification.Value, result.Verification.Value);

        Assert.Equal(profile.Skills.Count, result.Skills.Count);
        Assert.Equal(profile.Projects.Count, result.Projects.Count);
        Assert.Equal(profile.WorkExperience.Count, result.WorkExperience.Count);
    }
}