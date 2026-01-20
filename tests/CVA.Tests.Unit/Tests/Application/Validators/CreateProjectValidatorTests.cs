using CVA.Application.ProfileService;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="CreateProjectValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class CreateProjectValidatorTests
{
    private readonly CreateProjectValidator _validator = new();

    /// <summary>
    /// Purpose: Validate the Name property (required, max length 200).
    /// Should: Return validation error if empty or too long.
    /// When: Name is null, empty, whitespace or >200 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("   ", true)]
    [InlineCvaAutoData("Project Name", false)]
    [InlineCvaAutoData("A very long project name that reaches exactly two hundred characters mark to check boundary validation logic. This is just filler text to ensure we reach the required length for this test case. 12345", false)]
    [InlineCvaAutoData("A very long project name that exceeds two hundred characters mark for validation purposes test test test test test test test test test test test test test test test test test test test test test test test", true)]
    public void Name_Validation(string name, bool shouldHaveError, CreateProjectCommand baseCommand)
    {
        // Arrange
        var command = new CreateProjectCommand(Request: baseCommand.Request with { Name = name });

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
    [InlineCvaAutoData(null!, false)]
    [InlineCvaAutoData("", false)]
    [InlineCvaAutoData("Short description", false)]
    public void Description_Validation_Short(string? description, bool shouldHaveError, CreateProjectCommand baseCommand)
    {
        // Arrange
        var command = new CreateProjectCommand(Request: baseCommand.Request with { Description = description });

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
    [InlineCvaAutoData(null!, false)]
    [InlineCvaAutoData("", false)]
    [InlineCvaAutoData("invalid-url", true)]
    [InlineCvaAutoData("http://example.com/icon.png", false)]
    [InlineCvaAutoData("https://example.com/icon.png", false)]
    public void IconUrl_Validation(string? iconUrl, bool shouldHaveError, CreateProjectCommand baseCommand)
    {
        // Arrange
        var command = new CreateProjectCommand(Request: baseCommand.Request with { IconUrl = iconUrl });

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
    [InlineCvaAutoData("http://example.com", false)]
    [InlineCvaAutoData("https://example.com", false)]
    public void LinkUrl_Validation(string linkUrl, bool shouldHaveError, CreateProjectCommand baseCommand)
    {
        // Arrange
        var command = new CreateProjectCommand(Request: baseCommand.Request with { LinkUrl = linkUrl });

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
    public void TechStack_Validation_NotNull(CreateProjectCommand baseCommand)
    {
        // Arrange
        var command = new CreateProjectCommand(Request: baseCommand.Request with
        {
            TechStack = null!,
            IconUrl = "https://example.com/icon.png",
            LinkUrl = "https://example.com"
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(projectCommand => projectCommand.Request.TechStack);
    }
}
