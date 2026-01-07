namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Validator for the <see cref="UpdateProfileHeaderCommand"/>.
/// </summary>
public class UpdateProfileHeaderValidator : AbstractValidator<UpdateProfileHeaderCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProfileHeaderValidator"/> class.
    /// </summary>
    public UpdateProfileHeaderValidator()
    {
        RuleFor(command => command.Request.FirstName)
            .NotEmpty()
            .MaximumLength(100)
            .When(command => command.Request.FirstName != null);
        
        RuleFor(command => command.Request.LastName)
            .NotEmpty()
            .MaximumLength(100)
            .When(command => command.Request.LastName != null);
        
        RuleFor(command => command.Request.Role)
            .MaximumLength(200);
        RuleFor(command => command.Request.AvatarUrl)
            .Must(value => string.IsNullOrEmpty(value) || Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("Invalid Avatar URL");
        
        RuleFor(command => command.Request.YearsOfExperience)
            .GreaterThanOrEqualTo(0)
            .When(command => command.Request.YearsOfExperience.HasValue);

        RuleFor(command => command.Request.VerificationStatus)
            .MaximumLength(50);
    }
}