using CVA.Application.ProfileService;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="UpdateProfileSummaryValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class UpdateProfileSummaryValidatorTests
{
    private readonly UpdateProfileSummaryValidator _validator = new();

    /// <summary>
    /// Purpose: Validate the Summary property (max length 5000).
    /// Should: Return validation error if too long.
    /// When: Summary exceeds 5000 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(null!, false)]
    [InlineCvaAutoData("", false)]
    [InlineCvaAutoData("A valid summary", false)]
    [InlineCvaAutoData(5000, false)]
    [InlineCvaAutoData(5001, true)]
    public void Summary_Length_Validation(object input, bool shouldHaveError)
    {
        // Arrange
        var summary = input switch
        {
            int length => new string('a', length),
            string s => s,
            _ => null
        };

        var request = new UpdateProfileSummaryRequest { Summary = summary };
        var command = new UpdateProfileSummaryCommand(Request: request);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, summaryCommand => summaryCommand.Request.Summary, shouldHaveError);
    }
}
