namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Validator for the <see cref="DeleteProjectCommand"/>.
/// </summary>
public class DeleteProjectValidator : AbstractValidator<DeleteProjectCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProjectValidator"/> class.
    /// </summary>
    public DeleteProjectValidator()
    {
        RuleFor(command => command.ProjectId)
            .NotEmpty();
    }
}