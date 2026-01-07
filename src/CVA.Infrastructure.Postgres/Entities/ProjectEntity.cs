namespace CVA.Infrastructure.Postgres;

/// <summary>
/// EF Core persistence model for Project.
/// </summary>
internal sealed class ProjectEntity
{
    /// <summary>
    /// Unique identifier of the project.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Owner identifier.
    /// </summary>
    public Guid DeveloperProfileId { get; set; }

    /// <summary>
    /// Project name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Project description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Project icon URL.
    /// </summary>
    public string? IconUrl { get; set; }

    /// <summary>
    /// Project link URL.
    /// </summary>
    public string? LinkUrl { get; set; }

    /// <summary>
    /// Tech stack used.
    /// </summary>
    public List<string> TechStack { get; set; } = [];
}