namespace CVA.Application.Contracts;

/// <summary>
/// Represents a request to upsert a project in a developer's profile.
/// </summary>
public sealed record UpsertProjectRequest
{
    /// <summary>
    /// The name of the project.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// The description of the project.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The URL to the project's icon.
    /// </summary>
    public string? IconUrl { get; init; }

    /// <summary>
    /// The URL to the project's website or repository.
    /// </summary>
    public string? LinkUrl { get; init; }

    /// <summary>
    /// The technologies used in the project.
    /// </summary>
    public string[] TechStack { get; init; } = [];
}