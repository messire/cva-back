using CVA.Application.DeveloperProfiles;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="UpdateProjectValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class UpdateProjectValidatorTests
{
    private readonly UpdateProjectValidator _validator = new();

    /// <summary>
    /// Purpose: Validate the ProjectId property.
    /// Should: Return validation error if ProjectId is empty.
    /// When: ProjectId is default Guid.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(true)] // Empty Guid
    [InlineCvaAutoData(false)] // Non-empty Guid
    public void ProjectId_Validation(bool isEmpty, UpdateProjectCommand baseCommand)
    {
        // Arrange
        var command = baseCommand with
        {
            ProjectId = isEmpty ? Guid.Empty : Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, projectCommand => projectCommand.ProjectId, isEmpty);
    }

    /// <summary>
    /// Purpose: Validate the Name property (required, max length 200).
    /// Should: Return validation error if empty or too long.
    /// When: Name is null, empty, whitespace or >200 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("   ", true)]
    [InlineCvaAutoData("Project Name", false)]
    [InlineCvaAutoData("A very long project name that exceeds two hundred characters mark for validation purposes test test test test test test test test test test test test test test test test test test test test test test test", true)]
    public void Name_Validation(string name, bool shouldHaveError, UpdateProjectCommand baseCommand)
    {
        // Arrange
        var command = baseCommand with
        {
            Request = baseCommand.Request with { Name = name }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, projectCommand => projectCommand.Request.Name, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the Description property (max length 2000).
    /// Should: Return validation error if too long.
    /// When: Description > 2000 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(null, false)]
    [InlineCvaAutoData("", false)]
    public void Description_Validation(string? description, bool shouldHaveError, UpdateProjectCommand baseCommand)
    {
        // Arrange
        var command = baseCommand with
        {
            Request = baseCommand.Request with { Description = description }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, projectCommand => projectCommand.Request.Description, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the IconUrl property (must be absolute URI if present).
    /// Should: Return validation error if not a well-formed absolute URI.
    /// When: IconUrl is not a valid absolute URI.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(null, false)]
    [InlineCvaAutoData("invalid-url", true)]
    [InlineCvaAutoData("https://example.com/icon.png", false)]
    public void IconUrl_Validation(string? iconUrl, bool shouldHaveError, UpdateProjectCommand baseCommand)
    {
        // Arrange
        var command = baseCommand with
        {
            Request = baseCommand.Request with { IconUrl = iconUrl }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, projectCommand => projectCommand.Request.IconUrl, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the LinkUrl property (required, must be absolute URI).
    /// Should: Return validation error if empty or not a well-formed absolute URI.
    /// When: LinkUrl is null, empty, or not a valid absolute URI.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("invalid-url", true)]
    [InlineCvaAutoData("https://example.com", false)]
    public void LinkUrl_Validation(string linkUrl, bool shouldHaveError, UpdateProjectCommand baseCommand)
    {
        // Arrange
        var command = baseCommand with
        {
            Request = baseCommand.Request with { LinkUrl = linkUrl }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, projectCommand => projectCommand.Request.LinkUrl, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the TechStack property (not null).
    /// Should: Return validation error if TechStack is null.
    /// When: TechStack is null.
    /// </summary>
    [Theory, CvaAutoData]
    public void TechStack_Validation_NotNull(UpdateProjectCommand baseCommand)
    {
        // Arrange
        var command = baseCommand with
        {
            Request = baseCommand.Request with { TechStack = null! }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(projectCommand => projectCommand.Request.TechStack);
    }
}
