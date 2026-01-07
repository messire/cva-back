namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Validator for the <see cref="GetDeveloperProfileByIdQuery"/>.
/// </summary>
public class GetDeveloperProfileByIdValidator : AbstractValidator<GetDeveloperProfileByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetDeveloperProfileByIdValidator"/> class.
    /// </summary>
    public GetDeveloperProfileByIdValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty();
    }
}