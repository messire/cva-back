namespace CVA.Application.Contracts;

/// <summary>
/// Data transfer object that represents details of a work experience including company information, role,
/// description, location, duration, and associated achievements or technologies used.
/// </summary>
public record WorkDto
{
    /// <summary>
    /// The name of the company associated with the work experience.
    /// </summary>
    public string? CompanyName { get; init; }

    /// <summary>
    /// The professional position or job title held during the work experience.
    /// </summary>
    public string? Role { get; init; }

    /// <summary>
    /// A detailed overview of the responsibilities, projects, and tasks performed during the work experience.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The location where the work experience was based, such as a city, state, or remote.
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    /// The date on which the work experience began.
    /// </summary>
    public DateOnly? StartDate { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public DateOnly? EndDate { get; init; }

    /// <summary>
    /// A collection of notable accomplishments or milestones achieved during the work experience.
    /// </summary>
    public string[]? Achievements { get; init; }

    /// <summary>
    /// The collection of technologies, tools, or programming languages utilized during the work experience.
    /// </summary>
    public string[]? TechStack { get; init; }
}