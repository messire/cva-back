using CVA.Application.DeveloperProfiles;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="UpdateWorkExperienceValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class UpdateWorkExperienceValidatorTests
{
    private readonly UpdateWorkExperienceValidator _validator = new();

    /// <summary>
    /// Purpose: Validate the WorkExperienceId property.
    /// Should: Return validation error if WorkExperienceId is empty.
    /// When: WorkExperienceId is default Guid.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(true)] // Empty Guid
    [InlineCvaAutoData(false)] // Non-empty Guid
    public void WorkExperienceId_Validation(bool isEmpty, UpdateWorkExperienceCommand baseCommand)
    {
        // Arrange
        var command = baseCommand with
        {
            WorkExperienceId = isEmpty ? Guid.Empty : Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, experienceCommand => experienceCommand.WorkExperienceId, isEmpty);
    }

    /// <summary>
    /// Purpose: Validate the Company property (required, max length 200).
    /// Should: Return validation error if empty or too long.
    /// When: Company is empty or > 200 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("Company Name", false)]
    [InlineCvaAutoData(201, true)]
    public void Company_Validation(object input, bool shouldHaveError, UpdateWorkExperienceCommand baseCommand)
    {
        // Arrange
        var company = input switch
        {
            int length => new string('a', length),
            string s => s,
            _ => string.Empty
        };

        var command = baseCommand with
        {
            Request = baseCommand.Request with { Company = company }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, experienceCommand => experienceCommand.Request.Company, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the Role property (required, max length 200).
    /// Should: Return validation error if empty or too long.
    /// When: Role is empty or > 200 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("Developer", false)]
    [InlineCvaAutoData(201, true)]
    public void Role_Validation(object input, bool shouldHaveError, UpdateWorkExperienceCommand baseCommand)
    {
        // Arrange
        var role = input switch
        {
            int length => new string('a', length),
            string s => s,
            _ => string.Empty
        };

        var command = baseCommand with
        {
            Request = baseCommand.Request with { Role = role }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, experienceCommand => experienceCommand.Request.Role, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the Description property (max length 4000).
    /// Should: Return validation error if too long.
    /// When: Description > 4000 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(4001, true)]
    [InlineCvaAutoData("Some description", false)]
    public void Description_Validation(object input, bool shouldHaveError, UpdateWorkExperienceCommand baseCommand)
    {
        // Arrange
        var description = input switch
        {
            int length => new string('a', length),
            string s => s,
            _ => null
        };

        var command = baseCommand with
        {
            Request = baseCommand.Request with { Description = description }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, experienceCommand => experienceCommand.Request.Description, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate StartDate is not empty.
    /// Should: Return error if StartDate is default.
    /// When: StartDate is default(DateOnly).
    /// </summary>
    [Theory, CvaAutoData]
    public void StartDate_NotEmpty_Validation(UpdateWorkExperienceCommand baseCommand)
    {
        // Arrange
        var command = baseCommand with
        {
            Request = baseCommand.Request with { StartDate = null }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(experienceCommand => experienceCommand.Request.StartDate);
    }

    /// <summary>
    /// Purpose: Validate EndDate is greater than or equal to StartDate.
    /// Should: Return error if EndDate is earlier than StartDate.
    /// When: EndDate < StartDate.
    /// </summary>
    [Theory, CvaAutoData]
    public void EndDate_Range_Validation(UpdateWorkExperienceCommand baseCommand)
    {
        // Arrange
        var startDate = new DateOnly(2020, 1, 1);
        var endDate = new DateOnly(2019, 1, 1);

        var command = baseCommand with
        {
            Request = baseCommand.Request with { StartDate = startDate, EndDate = endDate }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(experienceCommand => experienceCommand.Request.EndDate)
            .WithErrorMessage("End date must be greater than or equal to start date");
    }

    /// <summary>
    /// Purpose: Verify that complex object validation is triggered for Location.
    /// Should: Return validation errors for inner properties of LocationDto.
    /// When: Any property in Location is invalid.
    /// </summary>
    [Theory, CvaAutoData]
    public void Location_Should_Be_Validated(UpdateWorkExperienceCommand baseCommand)
    {
        // Arrange
        var command = baseCommand with
        {
            Request = baseCommand.Request with
            {
                Location = new LocationDto { City = new string('a', 101) }
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Request.Location.City");
    }

    /// <summary>
    /// Purpose: Validate TechStack is not null.
    /// Should: Return error if TechStack is null.
    /// When: TechStack is null.
    /// </summary>
    [Theory, CvaAutoData]
    public void TechStack_NotNull_Validation(UpdateWorkExperienceCommand baseCommand)
    {
        // Arrange
        var command = baseCommand with
        {
            Request = baseCommand.Request with { TechStack = null! }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(experienceCommand => experienceCommand.Request.TechStack);
    }
}
