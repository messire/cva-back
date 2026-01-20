namespace CVA.Application.ProfileService;

/// <summary>
/// Validator for the <see cref="CreateProfileCommand"/>.
/// </summary>
public sealed class CreateProfileValidator : AbstractValidator<CreateProfileCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProfileValidator"/> class.
    /// </summary>
    public CreateProfileValidator()
    {
        RuleFor(command => command.Request.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Request.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(command => command.Request.Role)
            .MaximumLength(200);

        RuleFor(command => command.Request.Summary)
            .MaximumLength(2000);

        RuleFor(command => command.Request.AvatarUrl)
            .Must(value => string.IsNullOrEmpty(value) || Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid Avatar URL");

        RuleFor(command => command.Request.Phone)
            .MaximumLength(30)
            .When(command => !string.IsNullOrEmpty(command.Request.Phone));

        RuleFor(command => command.Request.Website)
            .Must(value => string.IsNullOrEmpty(value) || Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid Website URL");


        RuleFor(command => command.Request.Location)
            .SetValidator(new LocationDtoValidator()!)
            .When(command => command.Request.Location != null);

        RuleFor(command => command.Request.SocialLinks)
            .SetValidator(new SocialLinksDtoValidator()!);
    }
}