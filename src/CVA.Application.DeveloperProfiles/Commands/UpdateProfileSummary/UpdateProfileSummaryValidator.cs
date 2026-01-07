namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Validator for the <see cref="UpdateProfileSummaryCommand"/>.
/// </summary>
public class UpdateProfileSummaryValidator : AbstractValidator<UpdateProfileSummaryCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProfileSummaryValidator"/> class.
    /// </summary>
    public UpdateProfileSummaryValidator()
    {
        RuleFor(command => command.Request.Summary)
            .MaximumLength(5000);
    }
}