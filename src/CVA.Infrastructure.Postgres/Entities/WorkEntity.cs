namespace CVA.Infrastructure.Postgres;

/// <summary>
/// EF Core owned an entity for work experience.
/// </summary>
internal sealed class WorkEntity
{
    /// <summary>
    /// The name of the company associated with the work experience.
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// The title or position held by the user at the company.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// A detailed description of the work experience, including responsibilities and tasks performed.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The location where the work experience took place, such as a city, state, or remote.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// The start and end dates of the work experience.
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The end date of the work experience.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// A collection of notable accomplishments or successes achieved during the work experience.
    /// </summary>
    public List<string> Achievements { get; set; } = [];

    /// <summary>
    /// The collection of technologies, tools, or frameworks used in the work experience.
    /// </summary>
    public List<string> TechStack { get; set; } = [];
}