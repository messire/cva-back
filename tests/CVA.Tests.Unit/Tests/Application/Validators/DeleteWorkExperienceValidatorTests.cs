using CVA.Application.DeveloperProfiles;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="DeleteWorkExperienceValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class DeleteWorkExperienceValidatorTests
{
    private readonly DeleteWorkExperienceValidator _validator = new();

    /// <summary>
    /// Purpose: Validate the WorkExperienceId property.
    /// Should: Return validation error if WorkExperienceId is empty.
    /// When: WorkExperienceId is default Guid.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(true)]
    [InlineCvaAutoData(false)]
    public void WorkExperienceId_Validation(bool isEmpty)
    {
        // Arrange
        var command = new DeleteWorkExperienceCommand(WorkExperienceId: isEmpty ? Guid.Empty : Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, experienceCommand => experienceCommand.WorkExperienceId, isEmpty);
    }
}
