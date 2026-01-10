using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.Models;

/// <summary>
/// Unit tests for the <see cref="User"/> domain model.
/// </summary>
[Trait(Layer.Domain, Category.Models)]
public class UserTests
{
    /// <summary>
    /// Purpose: Verify that CreateFromGoogle method correctly initializes a new user.
    /// Should: Set all properties correctly from provided arguments.
    /// When: Valid googleSubject, role and email are provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void Create_Should_Initialize_User(string googleSubject)
    {
        // Arrange
        var email = "test@example.com";

        // Act
        var user = User.CreateFromGoogle(email, googleSubject, UserRole.User);

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(googleSubject, user.GoogleSubject);
        Assert.Equal(UserRole.User, user.Role);
        Assert.Equal(email, user.Email.Value);
    }

    /// <summary>
    /// Purpose: Verify that FromPersistence method correctly reconstructs a user aggregate.
    /// Should: Restore all properties, including optional fields and collections.
    /// When: Valid state data is provided from persistence layer.
    /// </summary>
    [Theory, CvaAutoData]
    public void FromPersistence_Should_Reconstruct_User(Guid id, string googleSubject, DateTimeOffset createdAt, DateTimeOffset updateAt)
    {
        // Arrange
        var email = "test@example.com";

        // Act
        var user = User.FromPersistence(id, email, googleSubject, UserRole.User, createdAt, updateAt);

        // Assert
        Assert.Equal(id, user.Id);
        Assert.Equal(googleSubject, user.GoogleSubject);
        Assert.Equal(createdAt, user.CreatedAt);
        Assert.Equal(updateAt, user.UpdatedAt);
    }

    /// <summary>
    /// Purpose: Verify email change logic.
    /// Should: Update email property.
    /// When: Valid email string is provided.
    /// </summary>
    [Fact]
    public void ChangeRole_Should_Update_Role()
    {
        // Arrange
        var user = User.CreateFromGoogle("old@e.com", "N", UserRole.User);

        // Act
        user.ChangeRole(UserRole.Admin);

        // Assert
        Assert.Equal(UserRole.Admin, user.Role);
    }
}