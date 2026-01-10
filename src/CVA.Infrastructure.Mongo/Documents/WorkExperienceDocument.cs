namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Represents a MongoDB document for a work experience entry.
/// </summary>
internal sealed class WorkExperienceDocument
{
    /// <summary>
    /// The unique identifier for the work experience entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the company where the work experience took place.
    /// </summary>
    public string Company { get; set; } = null!;

    /// <summary>
    /// The location of the company where the work experience took place.
    /// </summary>
    public LocationDocument? Location { get; set; }

    /// <summary>
    /// The role or position held during the work experience.
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// A brief description of the work experience.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The start date of the work experience.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// The end date of the work experience, if applicable.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// The technology stack used during the work experience.
    /// </summary>
    public List<string> TechStack { get; set; } = [];
}