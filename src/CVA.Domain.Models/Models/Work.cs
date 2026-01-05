namespace CVA.Domain.Models;

/// <summary>
/// Represents a single work experience entry that belongs to a <see cref="User"/> aggregate.
/// Purpose: capture a user's professional experience as a value-rich entity inside the aggregate.
/// Notes: This is NOT an aggregate root and must not exist outside its owning user.
/// </summary>
public sealed partial class Work
{
    private readonly List<string> _achievements = [];
    private readonly List<string> _techStack = [];

    /// <summary>
    /// Company where the experience took place.
    /// </summary>
    public string? CompanyName { get; private set; }

    /// <summary>
    /// Role/title held by the user.
    /// </summary>
    public string? Role { get; private set; }

    /// <summary>
    /// Description of responsibilities and activities.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Location of the job.
    /// </summary>
    public string? Location { get; private set; }

    /// <summary>
    /// Start date of the experience.
    /// </summary>
    public DateOnly? StartDate { get; private set; }

    /// <summary>
    /// End date of the experience.
    /// Must be greater or equal to <see cref="StartDate"/> when both are specified.
    /// </summary>
    public DateOnly? EndDate { get; private set; }

    /// <summary>
    /// Achievements list (read-only view).
    /// </summary>
    public IReadOnlyList<string> Achievements => _achievements;

    /// <summary>
    /// Tech stack list (read-only view).
    /// </summary>
    public IReadOnlyList<string> TechStack => _techStack;

    /// <summary>
    /// EF/Mongo constructor.
    /// Purpose: required for ORMs/serializers.
    /// </summary>
    private Work()
    {
    }
}