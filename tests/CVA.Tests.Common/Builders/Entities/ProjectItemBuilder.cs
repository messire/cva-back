namespace CVA.Tests.Common;

/// <summary>
/// A builder class that creates instances of the <see cref="ProjectItem"/> type using the AutoFixture library.
/// </summary>
internal sealed class ProjectItemBuilder : ISpecimenBuilder
{
    /// <summary>
    /// A singleton instance of the <see cref="ProjectItemBuilder"/> class.
    /// </summary>
    public static readonly ISpecimenBuilder Instance = new ProjectItemBuilder();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(ProjectItem)) return new NoSpecimen();

        return ProjectItem.Create(
            id: (ProjectId)context.Resolve(typeof(ProjectId)),
            name: (ProjectName)context.Resolve(typeof(ProjectName)),
            description: (ProjectDescription)context.Resolve(typeof(ProjectDescription)),
            icon: (ProjectIcon)context.Resolve(typeof(ProjectIcon)),
            link: (ProjectLink)context.Resolve(typeof(ProjectLink)),
            techStack: (IEnumerable<TechTag>)context.Resolve(typeof(IEnumerable<TechTag>))
        );
    }
}