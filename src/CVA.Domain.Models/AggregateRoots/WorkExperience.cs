namespace CVA.Domain.Models;

public sealed partial class DeveloperProfile
{
    /// <summary>
    /// Adds a new work experience entry to the developer's profile.
    /// </summary>
    /// <param name="company">The name of the company where the work experience was gained.</param>
    /// <param name="location">The location of the company, including city and country. This parameter is optional.</param>
    /// <param name="role">The title of the role held during the work experience.</param>
    /// <param name="description">A description of the work performed in this role. This parameter is optional.</param>
    /// <param name="period">The date range representing the start and end dates of the work experience.</param>
    /// <param name="techStack">A collection of technology tags that represent the tools and technologies used in this role.</param>
    /// <param name="now">The current timestamp, used to update the profile's last modified time.</param>
    /// <returns>Returns the unique identifier of the added work experience.</returns>
    public WorkExperienceId AddWorkExperience(
        CompanyName company,
        Location? location,
        RoleTitle role,
        WorkDescription? description,
        DateRange period,
        IEnumerable<TechTag> techStack,
        DateTimeOffset now)
    {
        var id = new WorkExperienceId(Guid.NewGuid());
        var item = WorkExperienceItem.Create(id, company, location, role, description, period, techStack);
        _workExperience.Add(item);
        Touch(now);
        return id;
    }

    /// <summary>
    /// Updates an existing work experience entry in the developer's profile.
    /// </summary>
    /// <param name="id">The unique identifier of the work experience to be updated.</param>
    /// <param name="company">The updated name of the company associated with the work experience.</param>
    /// <param name="location">The updated location of the company, including city and country. This parameter is optional.</param>
    /// <param name="role">The updated title of the role held during the work experience.</param>
    /// <param name="description">The updated description of the work performed in this role. This parameter is optional.</param>
    /// <param name="period">The updated date range representing the start and end dates of the work experience.</param>
    /// <param name="techStack">The updated collection of technology tags that represent the tools and technologies used in this role.</param>
    /// <param name="now">The current timestamp, used to update the profile's last modified time.</param>
    public void UpdateWorkExperience(
        WorkExperienceId id,
        CompanyName company,
        Location? location,
        RoleTitle role,
        WorkDescription? description,
        DateRange period,
        IEnumerable<TechTag> techStack,
        DateTimeOffset now)
    {
        var item = FindWorkExperience(id);
        item.Update(company, location, role, description, period, techStack);
        Touch(now);
    }

    /// <summary>
    /// Removes a specified work experience entry from the developer's profile.
    /// </summary>
    /// <param name="id">The unique identifier of the work experience entry to be removed.</param>
    /// <param name="now">The current timestamp, used to update the profile's last modified time.</param>
    /// <returns>True if the work experience was removed; otherwise, false.</returns>
    public bool RemoveWorkExperience(WorkExperienceId id, DateTimeOffset now)
    {
        Ensure.NotEmpty(id.Value, nameof(id));
        var removed = _workExperience.RemoveAll(item => item.Id.Equals(id)) > 0;
        if (removed)
        {
            Touch(now);
        }
        return removed;
    }
    
    private WorkExperienceItem FindWorkExperience(WorkExperienceId id)
    {
        Ensure.NotEmpty(id.Value, nameof(id));
        var experienceItem = _workExperience.FirstOrDefault(item => item.Id.Equals(id));
        return experienceItem ?? throw new WorkExperienceNotFoundException(id);
    }
}