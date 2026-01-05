namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="WorkDtoValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class WorkDtoValidatorTests
{
    private readonly WorkDtoValidator _validator = new();

    /// <summary>
    /// Purpose: Verify that an empty DTO is considered valid.
    /// Should: Not return any validation errors.
    /// When: All properties in WorkDto are null or default.
    /// </summary>
    [Fact]
    public void Should_Not_Have_Errors_When_Dto_Is_Empty()
    {
        // Arrange
        var dto = new WorkDto();

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Purpose: Verify validation for valid date range.
    /// Should: Not return errors.
    /// When: EndDate is after StartDate.
    /// </summary>
    [Theory, CvaAutoData]
    public void Should_Not_Have_Errors_When_Dates_Are_Valid(WorkDto baseDto)
    {
        // Arrange
        var workDto = baseDto with
        {
            StartDate = new DateOnly(2020, 1, 1),
            EndDate = new DateOnly(2023, 1, 1)
        };

        // Act
        var result = _validator.TestValidate(workDto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(dto => dto.EndDate);
    }

    /// <summary>
    /// Purpose: Validate the maximum length constraint for CompanyName.
    /// Should: Return a validation error if length exceeds 100 characters.
    /// When: CompanyName length is greater than 100.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(100, false)]
    [InlineCvaAutoData(101, true)]
    public void CompanyName_Length_Validation(int length, bool shouldHaveError, WorkDto baseDto)
    {
        // Arrange
        var workDto = baseDto with
        {
            CompanyName = new string('a', length)
        };

        // Act
        var result = _validator.TestValidate(workDto);

        // Assert
        Helpers.AssertValidation(result, dto => dto.CompanyName, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the maximum length constraint for Role.
    /// Should: Return a validation error if length exceeds 100 characters.
    /// When: Role length is greater than 100.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(100, false)]
    [InlineCvaAutoData(101, true)]
    public void Role_Length_Validation(int length, bool shouldHaveError, WorkDto baseDto)
    {
        // Arrange
        var workDto = baseDto with
        {
            Role = new string('a', length)
        };

        // Act
        var result = _validator.TestValidate(workDto);

        // Assert
        Helpers.AssertValidation(result, dto => dto.Role, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the maximum length constraint for Location.
    /// Should: Return a validation error if length exceeds 200 characters.
    /// When: Location length is greater than 200.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(200, false)]
    [InlineCvaAutoData(201, true)]
    public void Location_Length_Validation(int length, bool shouldHaveError, WorkDto baseDto)
    {
        // Arrange
        var workDto = baseDto with
        {
            Location = new string('a', length)
        };

        // Act
        var result = _validator.TestValidate(workDto);

        // Assert
        Helpers.AssertValidation(result, dto => dto.Location, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Ensure the start date is not in the future.
    /// Should: Return a specific error message.
    /// When: StartDate is set to a future date.
    /// </summary>
    [Theory, CvaAutoData]
    public void StartDate_Should_Have_Error_When_In_Future(WorkDto baseDto)
    {
        // Arrange
        var workDto = baseDto with
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
        };

        // Act
        var result = _validator.TestValidate(workDto);

        // Assert
        result
            .ShouldHaveValidationErrorFor(dto => dto.StartDate)
            .WithErrorMessage("Start date cannot be in the future");
    }

    /// <summary>
    /// Purpose: Ensure today's date is valid for StartDate.
    /// Should: Not have validation error.
    /// When: StartDate is equal to DateTime.Now.
    /// </summary>
    [Theory, CvaAutoData]
    public void StartDate_Should_Not_Have_Error_When_Today(WorkDto baseDto)
    {
        // Arrange
        var workDto = baseDto with
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now)
        };

        // Act
        var result = _validator.TestValidate(workDto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(dto => dto.StartDate);
    }

    /// <summary>
    /// Purpose: Verify chronological order of work dates.
    /// Should: Return a specific error message.
    /// When: EndDate is earlier than StartDate.
    /// </summary>
    [Theory, CvaAutoData]
    public void EndDate_Should_Have_Error_When_Earlier_Than_StartDate(WorkDto baseDto)
    {
        // Arrange
        var workDto = baseDto with
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2022, 12, 31)
        };

        // Act
        var result = _validator.TestValidate(workDto);

        // Assert
        result
            .ShouldHaveValidationErrorFor(dto => dto.EndDate)
            .WithErrorMessage("End date cannot be earlier than start date");
    }

    /// <summary>
    /// Purpose: Ensure StartDate and EndDate can be the same day.
    /// Should: Not have validation error.
    /// When: StartDate and EndDate are identical.
    /// </summary>
    [Theory, CvaAutoData]
    public void EndDate_Should_Not_Have_Error_When_Equals_StartDate(WorkDto baseDto)
    {
        // Arrange
        var date = new DateOnly(2023, 1, 1);
        var workDto = baseDto with
        {
            StartDate = date,
            EndDate = date
        };

        // Act
        var result = _validator.TestValidate(workDto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(dto => dto.EndDate);
    }

    /// <summary>
    /// Purpose: Check the conditional validation for EndDate.
    /// Should: Not return an error for EndDate if StartDate is missing.
    /// When: StartDate is null and EndDate is provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void EndDate_Should_Not_Have_Error_When_StartDate_Is_Missing(WorkDto baseDto)
    {
        // Arrange
        var workDto = baseDto with
        {
            StartDate = null,
            EndDate = new DateOnly(2020, 1, 1)
        };

        // Act
        var result = _validator.TestValidate(workDto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(dto => dto.EndDate);
    }

    /// <summary>
    /// Purpose: Verify that null StartDate is allowed by the validator.
    /// Should: Not have validation error.
    /// When: StartDate is null.
    /// </summary>
    [Theory, CvaAutoData]
    public void StartDate_Should_Not_Have_Error_When_Null(WorkDto baseDto)
    {
        // Arrange
        var workDto = baseDto with
        {
            StartDate = null
        };

        // Act
        var result = _validator.TestValidate(workDto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(dto => dto.StartDate);
    }
}