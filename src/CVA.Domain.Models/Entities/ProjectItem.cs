namespace CVA.Domain.Models;

/// <summary>
/// Represents a project item.
/// </summary>
public sealed class ProjectItem
{
    private readonly List<TechTag> _techStack = new();
    
    /// <summary>
    /// The unique identifier of the project.
    /// </summary>
    public ProjectId Id { get; }

    /// <summary>
    /// The name of the project.
    /// </summary>
    public ProjectName Name { get; private set; }

    /// <summary>
    /// The description of the project.
    /// </summary>
    public ProjectDescription? Description { get; private set; }

    /// <summary>
    /// The icon associated with the project.
    /// </summary>
    public ProjectIcon? Icon { get; private set; }

    /// <summary>
    /// The link to the project.
    /// </summary>
    public ProjectLink Link { get; private set; }

    /// <summary>
    /// The technology stack used in the project.
    /// </summary>
    public IReadOnlyList<TechTag> TechStack => _techStack;

    private ProjectItem(
        ProjectId id,
        ProjectName name,
        ProjectDescription? description,
        ProjectIcon? icon,
        ProjectLink link,
        IEnumerable<TechTag> techStack)
    {
        Id = id;
        Name = name;
        Description = description;
        Icon = icon;
        Link = link;

        ReplaceTechStack(techStack);
    }

    /// <summary>
    /// Creates a new project item.
    /// </summary>
    /// <param name="id">The unique identifier of the project.</param>
    /// <param name="name">The name of the project.</param>
    /// <param name="description">The description of the project.</param>
    /// <param name="icon">The icon associated with the project.</param>
    /// <param name="link">The link to the project.</param>
    /// <param name="techStack">The technology stack used in the project.</param>
    /// <returns>A new <see cref="ProjectItem"/> instance initialized with the provided data.</returns>
    public static ProjectItem Create(
        ProjectId id,
        ProjectName name,
        ProjectDescription? description,
        ProjectIcon? icon,
        ProjectLink link,
        IEnumerable<TechTag> techStack)
    {
        Ensure.NotEmpty(id.Value, nameof(id));
        Ensure.NotNull(name, nameof(name));
        Ensure.NotNull(link, nameof(link));

        return new ProjectItem(id, name, description, icon, link, techStack);
    }

    /// <summary>
    /// Creates a <see cref="ProjectItem"/> instance from persisted data.
    /// </summary>
    /// <param name="id">The unique identifier of the project.</param>
    /// <param name="name">The name of the project.</param>
    /// <param name="description">The description of the project.</param>
    /// <param name="icon">The icon associated with the project.</param>
    /// <param name="link">The link to the project.</param>
    /// <param name="techStack">The technology stack used in the project.</param>
    /// <returns>A new <see cref="ProjectItem"/> instance initialized with the provided data.</returns>
    public static ProjectItem FromPersistence(
        ProjectId id,
        ProjectName name,
        ProjectDescription? description,
        ProjectIcon? icon,
        ProjectLink link,
        IEnumerable<TechTag> techStack)
    {
        Ensure.NotEmpty(id.Value, nameof(id));
        Ensure.NotNull(name, nameof(name));
        Ensure.NotNull(link, nameof(link));
        Ensure.NotNull(techStack, nameof(techStack));

        var techStackFiltered = techStack.Where(tag => tag is not null).Distinct().ToArray();
        return new ProjectItem(id, name, description, icon, link, techStackFiltered);
    }

    /// <summary>
    /// Updates the project item with the specified values.
    /// </summary>
    /// <param name="name">The updated name of the project.</param>
    /// <param name="description">The updated description of the project.</param>
    /// <param name="icon">The updated icon associated with the project.</param>
    /// <param name="link">The updated link to the project.</param>
    /// <param name="techStack">The updated technology stack used in the project.</param>
    public void Update(
        ProjectName name,
        ProjectDescription? description,
        ProjectIcon? icon,
        ProjectLink link,
        IEnumerable<TechTag> techStack)
    {
        Ensure.NotNull(name, nameof(name));
        Ensure.NotNull(link, nameof(link));
        Ensure.NotNull(techStack, nameof(techStack));

        Name = name;
        Description = description;
        Icon = icon;
        Link = link;

        ReplaceTechStack(techStack);
    }

    private void ReplaceTechStack(IEnumerable<TechTag> techStack)
    {
        var normalized = techStack.Where(techTag => techTag is not null).Distinct().ToArray();
        _techStack.Clear();
        _techStack.AddRange(normalized);
    }
}