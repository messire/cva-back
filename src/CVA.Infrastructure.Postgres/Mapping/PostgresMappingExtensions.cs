namespace CVA.Infrastructure.Postgres;

internal static class UserPostgresMappingExtensions
{
    public static UserEntity ToEntity(this User user)
        => new()
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Phone = user.Phone,
            Photo = user.Photo,
            Birthday = user.Birthday,
            SummaryInfo = user.SummaryInfo,
            Skills = user.Skills.ToList(),
            WorkExperience = user.WorkExperience.Select(ToEntity).ToList(),
        };

    public static WorkEntity ToEntity(this Work work)
        => new()
        {
            CompanyName = work.CompanyName,
            Role = work.Role,
            Description = work.Description,
            Location = work.Location,
            StartDate = work.StartDate,
            EndDate = work.EndDate,
            Achievements = work.Achievements.ToList(),
            TechStack = work.TechStack.ToList(),
        };

    public static User ToDomain(this UserEntity entity)
        => User.FromPersistence(
            id: entity.Id,
            name: entity.Name,
            surname: entity.Surname,
            email: entity.Email,
            phone: entity.Phone,
            photo: entity.Photo,
            birthday: entity.Birthday,
            summaryInfo: entity.SummaryInfo,
            skills: entity.Skills,
            workExperience: entity.WorkExperience.Select(ToDomain)
        );

    public static Work ToDomain(this WorkEntity entity)
        => Work.Create(
            companyName: entity.CompanyName,
            role: entity.Role,
            startDate: entity.StartDate,
            endDate: entity.EndDate,
            description: entity.Description,
            location: entity.Location,
            achievements: entity.Achievements,
            techStack: entity.TechStack
        );
}