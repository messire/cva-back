namespace CVA.Domain.Models;

/// <summary>
/// Represents a work experience item.
/// </summary>
public sealed class WorkExperienceItem
{
    private readonly List<TechTag> _techStack = new();

    /// <summary>
    /// The unique identifier of the work experience.
    /// </summary>
    public WorkExperienceId Id { get; }

    /// <summary>
    /// The name of the company where the work experience took place.
    /// </summary>
    public CompanyName Company { get; private set; }

    /// <summary>
    /// The location where the work experience took place.
    /// </summary>
    public Location? Location { get; private set; }

    /// <summary>
    /// The title of the role held during the work experience.
    /// </summary>
    public RoleTitle Role { get; private set; }

    /// <summary>
    /// The description of the work experience.
    /// </summary>
    public WorkDescription? Description { get; private set; }

    /// <summary>
    /// The duration of the work experience.
    /// </summary>
    public DateRange Period { get; private set; }

    /// <summary>
    /// The technology stack used during the work experience.
    /// </summary>
    public IReadOnlyList<TechTag> TechStack => _techStack;

    private WorkExperienceItem(
        WorkExperienceId id,
        CompanyName company,
        Location? location,
        RoleTitle role,
        WorkDescription? description,
        DateRange period,
        IEnumerable<TechTag> techStack)
    {
        Id = id;

        Company = company;
        Location = location;

        Role = role;
        Description = description;

        Period = period;

        ReplaceTechStack(techStack);
    }

    /// <summary>
    /// Creates a new work experience item.
    /// </summary>
    /// <param name="id">The unique identifier for the work experience item. </param>
    /// <param name="company">The name of the company where the work experience took place. </param>
    /// <param name="location">The location where the work experience took place. </param>
    /// <param name="role">The role or position held during the work experience. </param>
    /// <param name="description">A brief description of the work experience. </param>
    /// <param name="period">The duration of the work experience. </param>
    /// <param name="techStack">The technology stack used during the work experience. </param>
    /// <returns>The created work experience item. </returns>
    public static WorkExperienceItem Create(
        WorkExperienceId id,
        CompanyName company,
        Location? location,
        RoleTitle role,
        WorkDescription? description,
        DateRange period,
        IEnumerable<TechTag> techStack)
    {
        Ensure.NotEmpty(id.Value, nameof(id));
        Ensure.NotNull(company, nameof(company));
        Ensure.NotNull(period, nameof(period));
        Ensure.NotNull(techStack, nameof(techStack));

        return new WorkExperienceItem(id, company, location, role, description, period, techStack);
    }

    /// <summary>
    /// Restores a work experience item from persistence.
    /// </summary>
    /// <param name="id">The unique identifier for the work experience item.</param>
    /// <param name="company">The name of the company where the work experience took place.</param>
    /// <param name="location">The location of the work experience.</param>
    /// <param name="role">The role or position held during the work experience.</param>
    /// <param name="description">A detailed description of the work experience.</param>
    /// <param name="period">The time period during which the work experience occurred.</param>
    /// <param name="techStack">The collection of technology tags associated with the work experience.</param>
    /// <returns>The restored work experience item.</returns>
    public static WorkExperienceItem FromPersistence(
        WorkExperienceId id,
        CompanyName company,
        Location? location,
        RoleTitle role,
        WorkDescription? description,
        DateRange period,
        IEnumerable<TechTag> techStack)
    {
        Ensure.NotEmpty(id.Value, nameof(id));
        Ensure.NotNull(company, nameof(company));
        Ensure.NotNull(role, nameof(role));
        Ensure.NotNull(period, nameof(period));
        Ensure.NotNull(techStack, nameof(techStack));

        var techStackFiltered = techStack.Where(tag => tag is not null).Distinct().ToArray();
        return new WorkExperienceItem(id, company, location, role, description, period, techStackFiltered);
    }

    /// <summary>
    /// Updates the work experience item with the specified values.
    /// </summary>
    /// <param name="company">The name of the company where the work experience took place. </param>
    /// <param name="location">The location where the work experience took place. </param>
    /// <param name="role">The role or position held during the work experience. </param>
    /// <param name="description">A brief description of the work experience. </param>
    /// <param name="period">The duration of the work experience. </param>
    /// <param name="techStack">The technology stack used during the work experience. </param>
    public void Update(
        CompanyName company,
        Location? location,
        RoleTitle role,
        WorkDescription? description,
        DateRange period,
        IEnumerable<TechTag> techStack)
    {
        Ensure.NotNull(company, nameof(company));
        Ensure.NotNull(role, nameof(role));
        Ensure.NotNull(period, nameof(period));
        Ensure.NotNull(techStack, nameof(techStack));

        Company = company;
        Location = location;
        Role = role;
        Description = description;
        Period = period;

        ReplaceTechStack(techStack);
    }

    private void ReplaceTechStack(IEnumerable<TechTag> techStack)
    {
        var normalized = techStack.Where(techTag => techTag is not null).Distinct().ToArray();
        _techStack.Clear();
        _techStack.AddRange(normalized);
    }
}