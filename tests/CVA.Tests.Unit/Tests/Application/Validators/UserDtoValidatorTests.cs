namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="UserDtoValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class UserDtoValidatorTests
{
    private readonly UserDtoValidator _validator = new();
    /// <summary>
    /// Purpose: Validate the Name property (required, max length 100).
    /// Should: Return validation error if empty or too long.
    /// When: Name is null, empty, whitespace or >100 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("   ", true)]
    [InlineCvaAutoData("John", false)]
    [InlineCvaAutoData("A very long name that reaches exactly one hundred characters mark to check boundary validation logic", false)]
    [InlineCvaAutoData("A very long name that exceeds one hundred characters mark for validation purposes test test test test test", true)]
    public void Name_Validation(string name, bool shouldHaveError, UserDto baseDto)
    {
        // Arrange
        var userDto = baseDto with
        {
            Name = name
        };

        // Act
        var result = _validator.TestValidate(userDto);

        // Assert
        Helpers.AssertValidation(result, dto => dto.Name, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the Surname property (required, max length 100).
    /// Should: Return validation error if empty or too long.
    /// When: Surname is null, empty or > 100 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("Doe", false)]
    [InlineCvaAutoData("Boundary check: exactly one hundred characters string for surname validation purposes test test test", false)]
    [InlineCvaAutoData("A very long surname that exceeds one hundred characters mark for validation purposes test test test test test", true)]
    public void Surname_Validation(string surname, bool shouldHaveError, UserDto baseDto)
    {
        // Arrange
        var userDto = baseDto with
        {
            Surname = surname
        };

        // Act
        var result = _validator.TestValidate(userDto);

        // Assert
        Helpers.AssertValidation(result, dto => dto.Surname, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the Email property (required, format, max length 100).
    /// Should: Return validation error for invalid format or if empty.
    /// When: Email is empty, invalid format or doesn't follow email rules.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("invalid-email", true)]
    [InlineCvaAutoData("john@example.com", false)]
    [InlineCvaAutoData("too-long-email-that-exceeds-one-hundred-characters-at-the-example-domain-part-of-the-email-address.com", true)]
    public void Email_Validation(string email, bool shouldHaveError, UserDto baseDto)
    {
        // Arrange
        var userDto = baseDto with
        {
            Email = email
        };

        // Act
        var result = _validator.TestValidate(userDto);

        // Assert
        Helpers.AssertValidation(result, dto => dto.Email, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Ensure birthdate is not in the future.
    /// Should: Return error if date is future, no error if today or past.
    /// When: Birthdate is set to future date.
    /// </summary>
    [Theory, CvaAutoData]
    public void Birthdate_Future_Check(UserDto baseDto)
    {
        // Arrange
        var futureDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var userDto = baseDto with
        {
            Birthday = futureDate
        };

        // Act
        var result = _validator.TestValidate(userDto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Birthday)
            .WithErrorMessage("Birthdate cannot be in the future");
    }

    /// <summary>
    /// Purpose: Ensure null Birthdate is allowed.
    /// Should: Not return validation error.
    /// When: Birthdate is null.
    /// </summary>
    [Theory, CvaAutoData]
    public void Birthdate_Can_Be_Null(UserDto baseDto)
    {
        // Arrange
        var userDto = baseDto with
        {
            Birthday = null
        };

        // Act
        var result = _validator.TestValidate(userDto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(dto => dto.Birthday);
    }

    /// <summary>
    /// Purpose: Verify that complex object validation is triggered for WorkExperience.
    /// Should: Return validation errors for inner properties of WorkDto.
    /// When: Any item in WorkExperience collection is invalid.
    /// </summary>
    [Theory, CvaAutoData]
    public void WorkExperience_Items_Should_Be_Validated(UserDto baseDto, WorkDto invalidWork)
    {
        // Arrange
        var workDto = invalidWork with
        {
            CompanyName = new string('a', 101)
        };
        var userDto = baseDto with
        {
            WorkExperience = [workDto]
        };

        // Act
        var result = _validator.TestValidate(userDto);

        // Assert
        result.ShouldHaveValidationErrorFor("WorkExperience[0].CompanyName");
    }
}