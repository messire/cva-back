namespace CVA.Application.ProfileService;

/// <summary>
/// Validator for the <see cref="GetProfileByIdQuery"/>.
/// </summary>
public class GetProfileByIdValidator : AbstractValidator<GetProfileByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetProfileByIdValidator"/> class.
    /// </summary>
    public GetProfileByIdValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty();
    }
}