using CVA.Infrastructure.Mongo.Documents;

namespace CVA.Infrastructure.Mongo.Mapping;

/// <summary>
/// Mapping between domain models and Mongo persistence documents.
/// </summary>
internal static class UserMongoMappingExtensions
{
    /// <summary>
    /// Maps a domain <see cref="User"/> to a Mongo <see cref="UserDocument"/>.
    /// </summary>
    public static UserDocument ToDocument(this User user)
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
            WorkExperience = user.WorkExperience.Select(ToDocument).ToList(),
        };

    /// <summary>
    /// Maps a domain <see cref="Work"/> to a Mongo <see cref="WorkDocument"/>.
    /// </summary>
    public static WorkDocument ToDocument(this Work work)
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

    /// <summary>
    /// Maps a Mongo <see cref="UserDocument"/> to a domain <see cref="User"/>.
    /// </summary>
    public static User ToDomain(this UserDocument document)
        => User.FromPersistence(
            id: document.Id,
            name: document.Name,
            surname: document.Surname,
            email: document.Email,
            phone: document.Phone,
            photo: document.Photo,
            birthday: document.Birthday,
            summaryInfo: document.SummaryInfo,
            skills: document.Skills,
            workExperience: document.WorkExperience.Select(ToDomain)
        );

    /// <summary>
    /// Maps a Mongo <see cref="WorkDocument"/> to a domain <see cref="Work"/>.
    /// </summary>
    public static Work ToDomain(this WorkDocument document)
        => Work.Create(
            companyName: document.CompanyName,
            role: document.Role,
            startDate: document.StartDate,
            endDate: document.EndDate,
            description: document.Description,
            location: document.Location,
            achievements: document.Achievements,
            techStack: document.TechStack
        );
}