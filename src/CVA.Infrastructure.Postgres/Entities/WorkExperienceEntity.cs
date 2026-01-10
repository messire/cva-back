namespace CVA.Infrastructure.Postgres;

/// <summary>
/// EF Core persistence model for WorkExperience.
/// </summary>
internal sealed class WorkExperienceEntity
{
    /// <summary>
    /// Unique identifier of the work experience entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Owner identifier.
    /// </summary>
    public Guid DeveloperProfileId { get; set; }

    /// <summary>
    /// Company name.
    /// </summary>
    public string Company { get; set; } = null!;

    /// <summary>
    /// Work location.
    /// </summary>
    public LocationEntity? Location { get; set; }

    /// <summary>
    /// Role or position.
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// Description of responsibilities.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Start date of the period.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// End date of the period (null if current).
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// Tech stack used.
    /// </summary>
    public List<string> TechStack { get; set; } = [];
}