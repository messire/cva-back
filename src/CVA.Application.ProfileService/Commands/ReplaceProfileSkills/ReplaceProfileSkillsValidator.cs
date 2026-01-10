namespace CVA.Application.ProfileService;

/// <summary>
/// Validator for the <see cref="ReplaceProfileSkillsCommand"/>.
/// </summary>
public class ReplaceProfileSkillsValidator : AbstractValidator<ReplaceProfileSkillsCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReplaceProfileSkillsValidator"/> class.
    /// </summary>
    public ReplaceProfileSkillsValidator()
    {
        RuleFor(command => command.Skills)
            .NotNull();

        RuleForEach(command => command.Skills)
            .NotEmpty()
            .MaximumLength(100);
    }
}