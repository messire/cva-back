namespace CVA.Application.Validators;

/// <summary>
/// Validator class for <see cref="UserDto"/>.
/// </summary>
internal class UserDtoValidator : AbstractValidator<UserDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserDtoValidator"/> class.
    /// </summary>
    public UserDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100);

        RuleFor(dto => dto.Surname)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100);

        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage("Email is required")
            .MaximumLength(100)
            .EmailAddress().WithMessage("A valid email address is required");

        RuleFor(dto => dto.Birthday)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .When(dto => dto.Birthday.HasValue)
            .WithMessage("Birthdate cannot be in the future");

        RuleForEach(dto => dto.WorkExperience)
            .SetValidator(new WorkDtoValidator());
    }
}