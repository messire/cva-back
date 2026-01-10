namespace CVA.Application.Contracts;

/// <summary>
/// Represents a single work experience entry in a developer profile.
/// </summary>
public sealed record WorkExperienceDto
{
    /// <summary>
    /// Unique identifier of the work experience entry.
    /// </summary>
    public Guid? Id { get; init; }

    /// <summary>
    /// Company name.
    /// </summary>
    public string? Company { get; init; }

    /// <summary>
    /// Work location (city + country or free-form).
    /// </summary>
    public LocationDto? Location { get; init; }

    /// <summary>
    /// Role or position held.
    /// </summary>
    public string? Role { get; init; }

    /// <summary>
    /// Description of responsibilities and activities.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Start date of the work period.
    /// </summary>
    public DateOnly? StartDate { get; init; }

    /// <summary>
    /// End date of the work period (null if current).
    /// </summary>
    public DateOnly? EndDate { get; init; }

    /// <summary>
    /// Technologies used during this work experience.
    /// </summary>
    public string[]? TechStack { get; init; }
}