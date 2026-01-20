namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Represents a MongoDB document for a project entry.
/// </summary>
internal sealed class ProjectDocument
{
    /// <summary>
    /// The unique identifier of the project.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the project.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The description of the project.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The URL of the project icon.
    /// </summary>
    public string? IconUrl { get; set; }

    /// <summary>
    /// The URL of the project link.
    /// </summary>
    public string? LinkUrl { get; set; }

    /// <summary>
    /// The technology stack used in the project.
    /// </summary>
    public List<string> TechStack { get; set; } = [];
}