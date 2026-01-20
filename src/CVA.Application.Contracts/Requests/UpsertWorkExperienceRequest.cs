namespace CVA.Application.Contracts;

/// <summary>
/// Represents a request to upsert a work experience entry in a developer's profile.
/// </summary>
public sealed record UpsertWorkExperienceRequest
{
    /// <summary>
    /// The name of the company where the work experience was gained.
    /// </summary>
    public string? Company { get; init; }

    /// <summary>
    /// The location where the work experience was gained.
    /// </summary>
    public LocationDto? Location { get; init; }

    /// <summary>
    /// The role or position held during the work experience.
    /// </summary>
    public string? Role { get; init; }

    /// <summary>
    /// A brief description of the work experience.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The start date of the work experience.
    /// </summary>
    public DateOnly? StartDate { get; init; }

    /// <summary>
    /// The end date of the work experience.
    /// </summary>
    public DateOnly? EndDate { get; init; }

    /// <summary>
    /// The technologies used during the work experience.
    /// </summary>
    public string[] TechStack { get; init; } = [];
}