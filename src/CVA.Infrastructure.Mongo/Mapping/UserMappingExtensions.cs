namespace CVA.Infrastructure.Mongo;

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
            Email = user.Email.Value,
            Role = user.Role.ToString(),
            GoogleSubject = user.GoogleSubject,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
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
            email: document.Email,
            role: Enum.TryParse(document.Role, true, out UserRole role) ? role : UserRole.User,
            googleSubject: document.GoogleSubject,
            createdAt: document.CreatedAt,
            updatedAt: document.UpdatedAt
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

    /// <summary>
    /// Maps a domain <see cref="DeveloperProfile"/> to a Mongo <see cref="DeveloperProfileDocument"/>.
    /// </summary>
    /// <param name="profile">The domain model representing a developer's profile.</param>
    /// <returns>A MongoDB document representation of the developer's profile.</returns>
    public static DeveloperProfileDocument ToDocument(this DeveloperProfile profile)
        => new()
        {
            Id = profile.Id.Value,
            FirstName = profile.Name.FirstName,
            LastName = profile.Name.LastName,
            Role = profile.Role?.Value,
            Summary = profile.Summary?.Value,
            AvatarUrl = profile.Avatar?.ImageUrl.Value,
            OpenToWork = profile.OpenToWork.Value,
            YearsOfExperience = profile.YearsOfExperience.Value,
            Email = profile.Contact.Email.Value,
            Website = profile.Contact.Website?.Value,
            Location = profile.Contact.Location.ToDocument(),
            SocialLinks = profile.Social.ToDocument(),
            Skills = profile.Skills.Select(tag => tag.Value).ToList(),
            Projects = profile.Projects.Select(item => item.ToDocument()).ToList(),
            WorkExperience = profile.WorkExperience.Select(item => item.ToDocument()).ToList(),
            VerificationStatus = (int)profile.Verification.Value,
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt
        };

    /// <summary>
    /// Maps a Mongo <see cref="DeveloperProfileDocument"/> to a domain <see cref="DeveloperProfile"/>.
    /// </summary>
    /// <param name="document">The MongoDB document representing a developer's profile.</param>
    /// <returns>A domain model representation of the developer's profile.</returns>
    public static DeveloperProfile ToDomain(this DeveloperProfileDocument document)
    {
        var contactInfo = ContactInfo.Create(
            location: Location.TryFrom(document.Location?.City, document.Location?.Country),
            email: EmailAddress.From(document.Email),
            phone: PhoneNumber.TryFrom(document.Phone),
            website: Url.TryFrom(document.Website));
        var socials = SocialLinks.Create(document.SocialLinks?.LinkedIn, document.SocialLinks?.GitHub, document.SocialLinks?.Twitter, document.SocialLinks?.Telegram);

        var projects = document.Projects.Select(projectDocument =>
                ProjectItem.FromPersistence(
                    new ProjectId(projectDocument.Id),
                    ProjectName.From(projectDocument.Name),
                    ProjectDescription.TryFrom(projectDocument.Description),
                    ProjectIcon.TryFrom(projectDocument.IconUrl),
                    ProjectLink.From(projectDocument.LinkUrl),
                    projectDocument.TechStack.Select(TechTag.From)))
            .ToArray();

        var work = document.WorkExperience.Select(experienceDocument =>
                WorkExperienceItem.FromPersistence(
                    new WorkExperienceId(experienceDocument.Id),
                    CompanyName.From(experienceDocument.Company),
                    Location.TryFrom(experienceDocument.Location?.City, experienceDocument.Location?.Country),
                    RoleTitle.From(experienceDocument.Role),
                    WorkDescription.TryFrom(experienceDocument.Description),
                    DateRange.From(experienceDocument.StartDate, experienceDocument.EndDate),
                    experienceDocument.TechStack.Select(TechTag.From)))
            .ToArray();

        return DeveloperProfile.FromPersistence(
            id: new DeveloperId(document.Id),
            name: PersonName.From(document.FirstName, document.LastName),
            role: RoleTitle.TryFrom(document.Role),
            summary: ProfileSummary.TryFrom(document.Summary),
            avatar: Avatar.TryFrom(document.AvatarUrl),
            contact: contactInfo,
            social: socials,
            verification: new VerificationStatus((VerificationLevel)document.VerificationStatus),
            openToWork: new OpenToWorkStatus(document.OpenToWork),
            yearsOfExperience: YearsOfExperience.From(document.YearsOfExperience),
            skills: document.Skills.Select(SkillTag.From),
            projects: projects,
            workExperience: work,
            createdAt: document.CreatedAt,
            updatedAt: document.UpdatedAt);
    }

    private static LocationDocument? ToDocument(this Location? location)
        => location is null ? null : new LocationDocument { City = location.City, Country = location.Country };

    private static SocialLinksDocument ToDocument(this SocialLinks links)
        => new()
        {
            LinkedIn = links.LinkedIn?.Value,
            GitHub = links.GitHub?.Value,
            Twitter = links.Twitter?.Value,
            Telegram = links.Telegram?.Value
        };

    private static ProjectDocument ToDocument(this ProjectItem project)
        => new()
        {
            Id = project.Id.Value,
            Name = project.Name.Value,
            Description = project.Description?.Value,
            IconUrl = project.Icon?.ImageUrl.Value,
            LinkUrl = project.Link.Value?.Value,
            TechStack = project.TechStack.Select(tag => tag.Value).ToList()
        };

    private static WorkExperienceDocument ToDocument(this WorkExperienceItem work)
        => new()
        {
            Id = work.Id.Value,
            Company = work.Company.Value,
            Location = work.Location.ToDocument(),
            Role = work.Role?.Value ?? string.Empty,
            Description = work.Description?.Value,
            StartDate = work.Period.Start,
            EndDate = work.Period.End,
            TechStack = work.TechStack.Select(tag => tag.Value).ToList()
        };
}