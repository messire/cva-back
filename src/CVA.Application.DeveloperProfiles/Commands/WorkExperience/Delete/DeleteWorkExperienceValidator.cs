namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Validator for the <see cref="DeleteWorkExperienceCommand"/>.
/// </summary>
public class DeleteWorkExperienceValidator : AbstractValidator<DeleteWorkExperienceCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteWorkExperienceValidator"/> class.
    /// </summary>
    public DeleteWorkExperienceValidator()
    {
        RuleFor(command => command.WorkExperienceId)
            .NotEmpty();
    }
}