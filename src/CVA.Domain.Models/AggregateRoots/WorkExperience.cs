namespace CVA.Domain.Models;

public sealed partial class DeveloperProfile
{
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

    public void RemoveWorkExperience(WorkExperienceId id, DateTimeOffset now)
    {
        Ensure.NotEmpty(id.Value, nameof(id));
        _workExperience.RemoveAll(x => x.Id.Equals(id));
        Touch(now);
    }
    
    private WorkExperienceItem FindWorkExperience(WorkExperienceId id)
    {
        Ensure.NotEmpty(id.Value, nameof(id));
        var experienceItem = _workExperience.FirstOrDefault(item => item.Id.Equals(id));
        return experienceItem ?? throw new InvalidOperationException("Work experience not found.");
    }
}