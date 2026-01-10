namespace CVA.Tests.Common;

/// <summary>
/// A builder class that creates instances of the <see cref="WorkExperienceItem"/> type using the AutoFixture library.
/// </summary>
internal sealed class WorkExperienceItemBuilder : ISpecimenBuilder
{
    /// <summary>
    /// A singleton instance of the <see cref="WorkExperienceItemBuilder"/> class.
    /// </summary>
    public static readonly ISpecimenBuilder Instance = new WorkExperienceItemBuilder();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(WorkExperienceItem)) return new NoSpecimen();

        var start = (DateOnly)context.Resolve(typeof(DateOnly));
        var end = (DateOnly)context.Resolve(typeof(DateOnly));
        if (end < start) (start, end) = (end, start);

        return WorkExperienceItem.Create(
            id: (WorkExperienceId)context.Resolve(typeof(WorkExperienceId)),
            company: (CompanyName)context.Resolve(typeof(CompanyName)),
            location: (Location)context.Resolve(typeof(Location)),
            role: (RoleTitle)context.Resolve(typeof(RoleTitle)),
            description: (WorkDescription)context.Resolve(typeof(WorkDescription)),
            period: DateRange.From(start, end),
            techStack: (IEnumerable<TechTag>)context.Resolve(typeof(IEnumerable<TechTag>))
        );
    }
}