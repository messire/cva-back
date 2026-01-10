namespace CVA.Application.ProfileService;

/// <summary>
/// Validator for the <see cref="UpdateWorkExperienceCommand"/>.
/// </summary>
public class UpdateWorkExperienceValidator : AbstractValidator<UpdateWorkExperienceCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateWorkExperienceValidator"/> class.
    /// </summary>
    public UpdateWorkExperienceValidator()
    {
        RuleFor(command => command.WorkExperienceId)
            .NotEmpty();

        RuleFor(command => command.Request.Company)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.Request.Role)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.Request.Description)
            .MaximumLength(4000);

        RuleFor(command => command.Request.StartDate)
            .NotEmpty();

        RuleFor(command => command.Request.EndDate)
            .Must((model, endDate) => endDate == null || endDate >= model.Request.StartDate)
            .WithMessage("End date must be greater than or equal to start date");

        RuleFor(command => command.Request.TechStack)
            .NotNull();

        RuleFor(command => command.Request.Location)
            .SetValidator(new LocationDtoValidator()!)
            .When(command => command.Request.Location != null);
    }
}