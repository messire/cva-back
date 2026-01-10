namespace CVA.Application.ProfileService;

/// <summary>
/// Validator for the <see cref="UpdateProfileContactsCommand"/>.
/// </summary>
public class UpdateProfileContactsValidator : AbstractValidator<UpdateProfileContactsCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProfileContactsValidator"/> class.
    /// </summary>
    public UpdateProfileContactsValidator()
    {
        RuleFor(command => command.Request.Email)
            .EmailAddress()
            .When(command => !string.IsNullOrEmpty(command.Request.Email));

        RuleFor(command => command.Request.Phone)
            .MaximumLength(30)
            .When(command => !string.IsNullOrEmpty(command.Request.Phone));

        RuleFor(command => command.Request.Website)
            .Must(value => string.IsNullOrEmpty(value) || Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid Website URL");

        RuleFor(command => command.Request.SocialLinks)
            .SetValidator(new SocialLinksDtoValidator()!)
            .When(command => command.Request.SocialLinks != null);

        RuleFor(command => command.Request.Location)
            .SetValidator(new LocationDtoValidator()!)
            .When(command => command.Request.Location != null);
    }
}

/// <summary>
/// Validator for the <see cref="LocationDto"/>.
/// </summary>
public class LocationDtoValidator : AbstractValidator<LocationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LocationDtoValidator"/> class.
    /// </summary>
    public LocationDtoValidator()
    {
        RuleFor(dto => dto.City)
            .MaximumLength(100);

        RuleFor(dto => dto.Country)
            .MaximumLength(100);
    }
}

/// <summary>
/// Validator for the <see cref="SocialLinksDto"/>.
/// </summary>
public class SocialLinksDtoValidator : AbstractValidator<SocialLinksDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SocialLinksDtoValidator"/> class.
    /// </summary>
    public SocialLinksDtoValidator()
    {
        RuleFor(dto => dto.LinkedIn)
            .Must(value => string.IsNullOrEmpty(value) || Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid LinkedIn URL");

        RuleFor(dto => dto.GitHub)
            .Must(value => string.IsNullOrEmpty(value) || Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid GitHub URL");

        RuleFor(dto => dto.Twitter)
            .Must(value => string.IsNullOrEmpty(value) || Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid Twitter URL");

        RuleFor(dto => dto.Telegram)
            .Must(value => string.IsNullOrEmpty(value) || Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid Telegram URL");
    }
}