namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Validator for the <see cref="GetDeveloperProfilesCatalogQuery"/>.
/// </summary>
public class GetDeveloperProfilesCatalogValidator : AbstractValidator<GetDeveloperProfilesCatalogQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetDeveloperProfilesCatalogValidator"/> class.
    /// </summary>
    public GetDeveloperProfilesCatalogValidator()
    {
        RuleFor(query => query.Search)
            .MaximumLength(100);

        RuleForEach(query => query.Skills)
            .MaximumLength(50);

        RuleFor(query => query.VerificationStatus)
            .MaximumLength(50);
    }
}