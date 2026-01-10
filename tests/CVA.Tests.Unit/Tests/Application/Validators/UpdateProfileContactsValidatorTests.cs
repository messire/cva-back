using CVA.Application.ProfileService;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="UpdateProfileContactsValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class UpdateProfileContactsValidatorTests
{
    private readonly UpdateProfileContactsValidator _validator = new();

    /// <summary>
    /// Purpose: Validate the Email property format.
    /// Should: Return validation error for invalid format.
    /// When: Email is not a valid email address.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("invalid-email", true)]
    [InlineCvaAutoData("john@example.com", false)]
    [InlineCvaAutoData("", false)]
    [InlineCvaAutoData(null!, false)]
    public void Email_Validation(string? email, bool shouldHaveError, UpdateProfileContactsCommand baseCommand)
    {
        // Arrange
        var command = new UpdateProfileContactsCommand(Request: baseCommand.Request with { Email = email });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, contactsCommand => contactsCommand.Request.Email, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the Website property (must be absolute URI if present).
    /// Should: Return validation error if not a well-formed absolute URI.
    /// When: Website is not a valid absolute URI.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("invalid-url", true)]
    [InlineCvaAutoData("https://example.com", false)]
    [InlineCvaAutoData("", false)]
    [InlineCvaAutoData(null!, false)]
    public void Website_Validation(string? website, bool shouldHaveError, UpdateProfileContactsCommand baseCommand)
    {
        // Arrange
        var command = new UpdateProfileContactsCommand(Request: baseCommand.Request with { Website = website });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, contactsCommand => contactsCommand.Request.Website, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Verify that complex object validation is triggered for SocialLinks.
    /// Should: Return validation errors for inner properties of SocialLinksDto.
    /// When: Any property in SocialLinks is invalid.
    /// </summary>
    [Theory, CvaAutoData]
    public void SocialLinks_Should_Be_Validated(UpdateProfileContactsCommand baseCommand)
    {
        // Arrange
        var command = new UpdateProfileContactsCommand(Request: baseCommand.Request with
        {
            SocialLinks = new SocialLinksDto { LinkedIn = "invalid-url" }
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Request.SocialLinks.LinkedIn");
    }

    /// <summary>
    /// Purpose: Verify that complex object validation is triggered for Location.
    /// Should: Return validation errors for inner properties of LocationDto.
    /// When: Any property in Location is invalid.
    /// </summary>
    [Theory, CvaAutoData]
    public void Location_Should_Be_Validated(UpdateProfileContactsCommand baseCommand)
    {
        // Arrange
        var command = new UpdateProfileContactsCommand(Request: baseCommand.Request with
        {
            Location = new LocationDto { City = new string('a', 101) }
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Request.Location.City");
    }
}
