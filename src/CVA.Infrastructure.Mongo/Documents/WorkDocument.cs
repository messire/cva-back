namespace CVA.Infrastructure.Mongo.Documents;

/// <summary>
/// Represents a MongoDB document for a work experience entity.
/// </summary>
internal sealed class WorkDocument
{
    /// <summary>
    /// The name of the company where the individual gained work experience.
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// The title or position held by an individual during a specific work experience.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// A detailed explanation of the role or responsibilities held during the work experience.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The location where the work experience took place.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// The date when the work experience started.
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The date when the work experience ended, or null if it is ongoing.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// A collection of notable accomplishments or successes achieved during the work experience.
    /// </summary>
    public List<string> Achievements { get; set; } = [];

    /// <summary>
    /// The list of technologies.
    /// </summary>
    public List<string> TechStack { get; set; } = [];
}