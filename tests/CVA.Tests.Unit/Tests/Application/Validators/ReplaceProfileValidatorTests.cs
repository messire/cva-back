using CVA.Application.ProfileService;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="ReplaceProfileValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class ReplaceProfileValidatorTests
{
    private readonly ReplaceProfileValidator _validator = new();

    /// <summary>
    /// Purpose: Validate the FirstName property (required, max length 100).
    /// Should: Return validation error if empty or too long.
    /// When: FirstName is null, empty, whitespace or >100 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("   ", true)]
    [InlineCvaAutoData("John", false)]
    [InlineCvaAutoData("A very long name that reaches exactly one hundred characters mark to check boundary validation logic", false)]
    [InlineCvaAutoData("A very long name that exceeds one hundred characters mark for validation purposes test test test test test", true)]
    public void FirstName_Validation(string firstName, bool shouldHaveError, ReplaceProfileCommand baseCommand)
    {
        // Arrange
        var command = new ReplaceProfileCommand(Request: baseCommand.Request with { FirstName = firstName });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, profileCommand => profileCommand.Request.FirstName, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the LastName property (required, max length 100).
    /// Should: Return validation error if empty or too long.
    /// When: LastName is null, empty, whitespace or >100 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("   ", true)]
    [InlineCvaAutoData("Doe", false)]
    [InlineCvaAutoData("A very long name that reaches exactly one hundred characters mark to check boundary validation logic", false)]
    [InlineCvaAutoData("A very long name that exceeds one hundred characters mark for validation purposes test test test test test", true)]
    public void LastName_Validation(string lastName, bool shouldHaveError, ReplaceProfileCommand baseCommand)
    {
        // Arrange
        var command = new ReplaceProfileCommand(Request: baseCommand.Request with { LastName = lastName });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, profileCommand => profileCommand.Request.LastName, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the Email property (required, format).
    /// Should: Return validation error for invalid format or if empty.
    /// When: Email is empty or invalid format.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("invalid-email", true)]
    [InlineCvaAutoData("john@example.com", false)]
    public void Email_Validation(string email, bool shouldHaveError, ReplaceProfileCommand baseCommand)
    {
        // Arrange
        var command = new ReplaceProfileCommand(Request: baseCommand.Request with { Email = email });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, profileCommand => profileCommand.Request.Email, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the Website property (must be absolute URI if present).
    /// Should: Return validation error if not a well-formed absolute URI.
    /// When: Website is not a valid absolute URI.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(null!, false)]
    [InlineCvaAutoData("", false)]
    [InlineCvaAutoData("invalid-url", true)]
    [InlineCvaAutoData("https://example.com", false)]
    public void Website_Validation(string? website, bool shouldHaveError, ReplaceProfileCommand baseCommand)
    {
        // Arrange
        var command = new ReplaceProfileCommand(Request: baseCommand.Request with { Website = website });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, profileCommand => profileCommand.Request.Website, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Verify that complex object validation is triggered for Projects.
    /// Should: Return validation errors for inner properties of ProjectDto.
    /// When: Any item in Projects collection is invalid.
    /// </summary>
    [Theory, CvaAutoData]
    public void Projects_Items_Should_Be_Validated(ReplaceProfileCommand baseCommand, ProjectDto invalidProject)
    {
        // Arrange
        var projectDto = invalidProject with
        {
            Name = ""
        };
        var command = new ReplaceProfileCommand(Request: baseCommand.Request with { Projects = [projectDto] });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Request.Projects[0].Name");
    }

    /// <summary>
    /// Purpose: Verify that complex object validation is triggered for WorkExperience.
    /// Should: Return validation errors for inner properties of WorkExperienceDto.
    /// When: Any item in WorkExperience collection is invalid.
    /// </summary>
    [Theory, CvaAutoData]
    public void WorkExperience_Items_Should_Be_Validated(ReplaceProfileCommand baseCommand, WorkExperienceDto invalidWork)
    {
        // Arrange
        var workDto = invalidWork with
        {
            Company = ""
        };
        var command = new ReplaceProfileCommand(Request: baseCommand.Request with { WorkExperience = [workDto] });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Request.WorkExperience[0].Company");
    }
}
