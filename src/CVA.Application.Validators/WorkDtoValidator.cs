namespace CVA.Application.Validators;

/// <summary>
/// Validator class for <see cref="WorkDto"/>.
/// </summary>
internal class WorkDtoValidator : AbstractValidator<WorkDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkDtoValidator"/> class.
    /// </summary>
    public WorkDtoValidator()
    {
        RuleFor(dto => dto.CompanyName)
            .MaximumLength(100);

        RuleFor(dto => dto.Role)
            .MaximumLength(100);

        RuleFor(dto => dto.Location)
            .MaximumLength(200);

        RuleFor(dto => dto.StartDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .When(dto => dto.StartDate.HasValue)
            .WithMessage("Start date cannot be in the future");

        RuleFor(dto => dto.EndDate)
            .GreaterThanOrEqualTo(dto => dto.StartDate!.Value)
            .When(dto => dto.StartDate.HasValue && dto.EndDate.HasValue)
            .WithMessage("End date cannot be earlier than start date");
    }
}