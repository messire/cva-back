using CVA.Application.DeveloperProfiles;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="DeleteProjectValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class DeleteProjectValidatorTests
{
    private readonly DeleteProjectValidator _validator = new();

    /// <summary>
    /// Purpose: Validate the ProjectId property.
    /// Should: Return validation error if ProjectId is empty.
    /// When: ProjectId is default Guid.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(true)] // Empty Guid
    [InlineCvaAutoData(false)] // Non-empty Guid
    public void ProjectId_Validation(bool isEmpty)
    {
        // Arrange
        var command = new DeleteProjectCommand(ProjectId: isEmpty ? Guid.Empty : Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, projectCommand => projectCommand.ProjectId, isEmpty);
    }
}
