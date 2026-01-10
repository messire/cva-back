namespace CVA.Application.Contracts;

/// <summary>
/// Represents a project within a developer profile.
/// </summary>
public sealed record ProjectDto
{
    /// <summary>
    /// Project identifier.
    /// </summary>
    public Guid? Id { get; init; }

    /// <summary>
    /// Project name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Project description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Project icon URL.
    /// </summary>
    public string? IconUrl { get; init; }

    /// <summary>
    /// Optional project link.
    /// </summary>
    public string? LinkUrl { get; init; }

    /// <summary>
    /// Technologies used in the project.
    /// </summary>
    public string[]? TechStack { get; init; }
}