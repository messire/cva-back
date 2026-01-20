using CVA.Application.ProfileService;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for the <see cref="ReplaceProfileSkillsValidator"/> class.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public class ReplaceProfileSkillsValidatorTests
{
    private readonly ReplaceProfileSkillsValidator _validator = new();

    /// <summary>
    /// Purpose: Validate that Skills collection is not null.
    /// Should: Return validation error if Skills is null.
    /// When: Skills is null.
    /// </summary>
    [Fact]
    public void Skills_NotNull_Validation()
    {
        // Arrange
        var command = new ReplaceProfileSkillsCommand(Skills: null!);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(skillsCommand => skillsCommand.Skills);
    }

    /// <summary>
    /// Purpose: Validate that individual skills are not empty and within length limits.
    /// Should: Return validation error if any skill is empty or too long.
    /// When: Skill is empty string or > 100 characters.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("", true)]
    [InlineCvaAutoData("   ", true)]
    [InlineCvaAutoData("C#", false)]
    [InlineCvaAutoData("A very long skill name that exceeds one hundred characters limit for validation purposes test test test test", true)]
    public void Individual_Skill_Validation(string skill, bool shouldHaveError)
    {
        // Arrange
        var command = new ReplaceProfileSkillsCommand(Skills: [skill]);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        Helpers.AssertValidation(result, "Skills[0]", shouldHaveError);
    }
}
