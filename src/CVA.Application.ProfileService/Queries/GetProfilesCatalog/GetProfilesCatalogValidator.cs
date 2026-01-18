namespace CVA.Application.ProfileService;

/// <summary>
/// Validator for the <see cref="GetProfilesCatalogQuery"/>.
/// </summary>
public class GetProfilesCatalogValidator : AbstractValidator<GetProfilesCatalogQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetProfilesCatalogValidator"/> class.
    /// </summary>
    public GetProfilesCatalogValidator()
    {
        RuleFor(query => query.Search)
            .MaximumLength(100);

        RuleForEach(query => query.Skills)
            .MaximumLength(50);

        RuleFor(query => query.VerificationStatus)
            .MaximumLength(50);
    }
}