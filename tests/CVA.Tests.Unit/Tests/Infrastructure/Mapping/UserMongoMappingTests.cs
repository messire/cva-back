using CVA.Domain.Models;
using CVA.Infrastructure.Mongo;

namespace CVA.Tests.Unit.Infrastructure.Mapping;

/// <summary>
/// Unit tests for <see cref="UserMongoMappingExtensions"/>.
/// </summary>
[Trait(Layer.Infrastructure, Category.Mapping)]
public sealed class UserMongoMappingTests
{
    private static UserDocument WrapToDocument(User user) => user.ToDocument();
    private static User WrapToDomain(UserDocument document) => document.ToDomain();
    private static DeveloperProfileDocument WrapToDocument(DeveloperProfile profile) => profile.ToDocument();
    private static DeveloperProfile WrapToDomain(DeveloperProfileDocument document) => document.ToDomain();

    /// <summary>
    /// Verifies that <see cref="User"/> can be correctly mapped to <see cref="UserDocument"/>.
    /// </summary>
    [Theory, CvaAutoData]
    public void ToDocument_ShouldMapUserToUserDocument(User user)
    {
        // Act
        var document = WrapToDocument(user);

        // Assert
        Assert.Equal(user.Id, document.Id);
        Assert.Equal(user.GoogleSubject, document.GoogleSubject);
        Assert.Equal(user.Email.Value, document.Email);
        Assert.Equal(user.Role.ToString(), document.Role);
        Assert.Equal(user.CreatedAt, document.CreatedAt);
        Assert.Equal(user.UpdatedAt, document.UpdatedAt);
    }

    /// <summary>
    /// Verifies that <see cref="UserDocument"/> can be correctly mapped back to <see cref="User"/>.
    /// </summary>
    [Theory, CvaAutoData]
    public void ToDomain_ShouldMapUserDocumentToUser(User user)
    {
        // Arrange
        var document = WrapToDocument(user);

        // Act
        var result = WrapToDomain(document);

        // Assert
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.GoogleSubject, result.GoogleSubject);
        Assert.Equal(user.Email.Value, result.Email.Value);
        Assert.Equal(user.Role, result.Role);
        Assert.Equal(user.CreatedAt, result.CreatedAt);
        Assert.Equal(user.UpdatedAt, result.UpdatedAt);
    }

    /// <summary>
    /// Verifies that <see cref="DeveloperProfile"/> can be correctly mapped to <see cref="DeveloperProfileDocument"/>.
    /// </summary>
    [Theory, CvaAutoData]
    public void ToDocument_ShouldMapDeveloperProfileToDocument(DeveloperProfile profile)
    {
        // Act
        var document = WrapToDocument(profile);

        // Assert
        Assert.Equal(profile.Id.Value, document.Id);
        Assert.Equal(profile.Name.FirstName, (string?)document.FirstName);
        Assert.Equal(profile.Name.LastName, (string?)document.LastName);
        Assert.Equal(profile.Role?.Value, document.Role);
        Assert.Equal(profile.Summary?.Value, document.Summary);
        Assert.Equal(profile.Avatar?.ImageUrl.Value, document.AvatarUrl);
        Assert.Equal(profile.OpenToWork.Value, document.OpenToWork);
        Assert.Equal(profile.Contact.Email.Value, (string?)document.Email);
        Assert.Equal(profile.Contact.Website?.Value, document.Website);
        Assert.Equal((int)profile.Verification.Value, document.VerificationStatus);

        Assert.Equal(profile.Skills.Count, document.Skills.Count);
        Assert.Equal(profile.Projects.Count, document.Projects.Count);
        Assert.Equal(profile.WorkExperience.Count, document.WorkExperience.Count);
    }

    /// <summary>
    /// Verifies that <see cref="DeveloperProfileDocument"/> can be correctly mapped back to <see cref="DeveloperProfile"/>.
    /// </summary>
    [Theory, CvaAutoData]
    public void ToDomain_ShouldMapDeveloperProfileDocumentToDomain(DeveloperProfile profile)
    {
        // Arrange
        var document = WrapToDocument(profile);

        // Act
        var result = WrapToDomain(document);

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