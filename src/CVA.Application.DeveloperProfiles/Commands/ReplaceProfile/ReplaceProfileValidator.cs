namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Validator for the <see cref="ReplaceProfileCommand"/>.
/// </summary>
public class ReplaceProfileValidator : AbstractValidator<ReplaceProfileCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReplaceProfileValidator"/> class.
    /// </summary>
    public ReplaceProfileValidator()
    {
        RuleFor(command => command.Request.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Request.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Request.Role)
            .MaximumLength(200);

        RuleFor(command => command.Request.Summary)
            .MaximumLength(5000);

        RuleFor(command => command.Request.AvatarUrl)
            .Must(value => string.IsNullOrEmpty(value) || Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid Avatar URL");

        RuleFor(command => command.Request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(command => command.Request.Website)
            .Must(value => string.IsNullOrEmpty(value) || Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid Website URL");

        RuleFor(command => command.Request.Location)
            .NotNull()
            .SetValidator(new LocationDtoValidator()!);

        RuleFor(command => command.Request.SocialLinks)
            .NotNull()
            .SetValidator(new SocialLinksDtoValidator()!);

        RuleFor(command => command.Request.Skills)
            .NotNull();

        RuleForEach(command => command.Request.Skills)
            .NotEmpty();

        RuleFor(command => command.Request.Projects)
            .NotNull();

        RuleForEach(command => command.Request.Projects)
            .SetValidator(new ProjectDtoValidator());

        RuleFor(command => command.Request.WorkExperience)
            .NotNull();

        RuleForEach(command => command.Request.WorkExperience)
            .SetValidator(new WorkExperienceDtoValidator());
    }
}

/// <summary>
/// Validator for the <see cref="ProjectDto"/>.
/// </summary>
public class ProjectDtoValidator : AbstractValidator<ProjectDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectDtoValidator"/> class.
    /// </summary>
    public ProjectDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(dto => dto.LinkUrl)
            .NotEmpty()
            .Must(value => Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid Link URL");
    }
}

/// <summary>
/// Validator for the <see cref="WorkExperienceDto"/>.
/// </summary>
public class WorkExperienceDtoValidator : AbstractValidator<WorkExperienceDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkExperienceDtoValidator"/> class.
    /// </summary>
    public WorkExperienceDtoValidator()
    {
        RuleFor(dto => dto.Company)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(dto => dto.Role)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(dto => dto.StartDate)
            .NotEmpty();

        RuleFor(dto => dto.EndDate)
            .Must((model, endDate) => endDate == null || endDate >= model.StartDate)
            .WithMessage("End date must be greater than or equal to start date");

        RuleFor(dto => dto.Location)
            .SetValidator(new LocationDtoValidator()!)
            .When(dto => dto.Location != null);
    }
}