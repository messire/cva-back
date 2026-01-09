using CVA.Application.ProfileService;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="UpdateProfileHeaderValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class UpdateProfileHeaderValidatorTests
{
    private readonly UpdateProfileHeaderValidator _validator = new();

    /// <summary>
    /// Purpose: Validate the FirstName property (max length 100, not empty if provided).
    /// Should: Return validation error if empty or too long.
    /// When: FirstName is empty, whitespace or >100 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("   ", true)]
    [InlineCvaAutoData("John", false)]
    [InlineCvaAutoData("A very long name that reaches exactly one hundred characters mark to check boundary validation logic", false)]
    [InlineCvaAutoData("A very long name that exceeds one hundred characters mark for validation purposes test test test test test", true)]
    public void FirstName_Validation(string firstName, bool shouldHaveError, UpdateProfileHeaderCommand baseCommand)
    {
        // Arrange
        var command = new UpdateProfileHeaderCommand(Request: baseCommand.Request with { FirstName = firstName });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, headerCommand => headerCommand.Request.FirstName, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the AvatarUrl property (must be absolute URI if present).
    /// Should: Return validation error if not a well-formed absolute URI.
    /// When: AvatarUrl is not a valid absolute URI.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("invalid-url", true)]
    [InlineCvaAutoData("https://example.com/avatar.png", false)]
    [InlineCvaAutoData("", false)]
    [InlineCvaAutoData(null, false)]
    public void AvatarUrl_Validation(string? avatarUrl, bool shouldHaveError, UpdateProfileHeaderCommand baseCommand)
    {
        // Arrange
        var command = new UpdateProfileHeaderCommand(Request: baseCommand.Request with { AvatarUrl = avatarUrl });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, headerCommand => headerCommand.Request.AvatarUrl, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the YearsOfExperience property (must be >= 0).
    /// Should: Return validation error if negative.
    /// When: YearsOfExperience is negative.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(-1, true)]
    [InlineCvaAutoData(0, false)]
    [InlineCvaAutoData(5, false)]
    public void YearsOfExperience_Validation(int? years, bool shouldHaveError, UpdateProfileHeaderCommand baseCommand)
    {
        // Arrange
        var command = new UpdateProfileHeaderCommand(Request: baseCommand.Request with { YearsOfExperience = years });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, headerCommand => headerCommand.Request.YearsOfExperience, shouldHaveError);
    }
}
