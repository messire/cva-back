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

        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(query => query.SortField)
            .Must(BeValidSortField)
            .WithMessage("Unsupported sortField.");

        RuleFor(query => query.SortOrder)
            .Must(BeValidSortOrder)
            .WithMessage("Unsupported sortOrder.");

        RuleFor(query => query.VerificationStatus)
            .Must(BeValidVerificationStatus)
            .WithMessage("Unsupported verificationStatus.");
    }

    private static bool BeValidSortField(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return true;
        var v = value.Trim();
        return v is ProfilesSortFields.UpdatedAt or ProfilesSortFields.Name or ProfilesSortFields.Id;
    }

    private static bool BeValidSortOrder(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return true;
        var v = value.Trim();
        return v is SortOrders.Asc or SortOrders.Desc;
    }

    private static bool BeValidVerificationStatus(string? value)
        => string.IsNullOrWhiteSpace(value)
           || Enum.TryParse<VerificationLevel>(value.Trim(), true, out _);
}