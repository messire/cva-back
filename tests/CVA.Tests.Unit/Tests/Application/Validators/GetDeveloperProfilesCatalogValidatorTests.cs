using CVA.Application.DeveloperProfiles;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="GetDeveloperProfilesCatalogValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class GetDeveloperProfilesCatalogValidatorTests
{
    private readonly GetDeveloperProfilesCatalogValidator _validator = new();

    /// <summary>
    /// Purpose: Validate the Search property length.
    /// Should: Return validation error if Search > 100 characters.
    /// When: Search length exceeds 100.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(100, false)]
    [InlineCvaAutoData(101, true)]
    public void Search_Length_Validation(int length, bool shouldHaveError, GetDeveloperProfilesCatalogQuery baseQuery)
    {
        // Arrange
        var query = baseQuery with
        {
            Search = new string('a', length)
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        Helpers.AssertValidation(result, catalogQuery => catalogQuery.Search, shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate individual Skills length.
    /// Should: Return validation error if any skill > 50 characters.
    /// When: Skill length exceeds 50.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(50, false)]
    [InlineCvaAutoData(51, true)]
    public void Skills_Item_Length_Validation(int length, bool shouldHaveError, GetDeveloperProfilesCatalogQuery baseQuery)
    {
        // Arrange
        var query = baseQuery with
        {
            Skills = [new string('a', length)]
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        Helpers.AssertValidation(result, "Skills[0]", shouldHaveError);
    }

    /// <summary>
    /// Purpose: Validate the VerificationStatus property length.
    /// Should: Return validation error if VerificationStatus > 50 characters.
    /// When: VerificationStatus length exceeds 50.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(50, false)]
    [InlineCvaAutoData(51, true)]
    public void VerificationStatus_Length_Validation(int length, bool shouldHaveError, GetDeveloperProfilesCatalogQuery baseQuery)
    {
        // Arrange
        var query = baseQuery with
        {
            VerificationStatus = new string('a', length)
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        Helpers.AssertValidation(result, catalogQuery => catalogQuery.VerificationStatus, shouldHaveError);
    }
}
