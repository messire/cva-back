namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Validator for the <see cref="CreateProjectCommand"/>.
/// </summary>
public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProjectValidator"/> class.
    /// </summary>
    public CreateProjectValidator()
    {
        RuleFor(command => command.Request.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.Request.Description)
            .MaximumLength(2000);

        RuleFor(command => command.Request.IconUrl)
            .Must(value => string.IsNullOrEmpty(value) || Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid Icon URL");

        RuleFor(command => command.Request.LinkUrl)
            .NotEmpty()
            .Must(uriString => Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
            .WithMessage("Invalid Link URL");

        RuleFor(command => command.Request.TechStack)
            .NotNull();
    }
}