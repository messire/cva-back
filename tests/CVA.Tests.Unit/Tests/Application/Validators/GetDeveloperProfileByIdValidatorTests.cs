using CVA.Application.DeveloperProfiles;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="GetDeveloperProfileByIdValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class GetDeveloperProfileByIdValidatorTests
{
    private readonly GetDeveloperProfileByIdValidator _validator = new();

    /// <summary>
    /// Purpose: Validate the Id property.
    /// Should: Return validation error if Id is empty.
    /// When: Id is default Guid.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(true)]
    [InlineCvaAutoData(false)]
    public void Id_Validation(bool isEmpty)
    {
        // Arrange
        var query = new GetDeveloperProfileByIdQuery(Id: isEmpty ? Guid.Empty : Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        Helpers.AssertValidation(result, byIdQuery => byIdQuery.Id, isEmpty);
    }
}
