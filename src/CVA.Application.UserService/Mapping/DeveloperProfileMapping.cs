namespace CVA.Application.UserService;

/// <summary>
/// Provides extension methods for mapping between developer profile domain models and data transfer objects.
/// </summary>
internal static class DeveloperProfileMapping
{
    /// <summary>
    /// Converts a <see cref="Location"/> domain model to a <see cref="LocationDto"/>.
    /// </summary>
    /// <param name="model">The location domain model.</param>
    /// <returns>A new instance of <see cref="LocationDto"/>.</returns>
    public static LocationDto ToDto(this Location model)
        => new()
        {
            City = model.City,
            Country = model.Country
        };

    /// <summary>
    /// Converts a <see cref="LocationDto"/> to a <see cref="Location"/> domain model.
    /// </summary>
    /// <param name="dto">The location data transfer object.</param>
    /// <returns>A new instance of <see cref="Location"/> domain model.</returns>
    public static Location ToModel(this LocationDto dto)
        => Location.From(dto.City ?? string.Empty, dto.Country ?? string.Empty);

    /// <summary>
    /// Converts a <see cref="SocialLinks"/> domain model to a <see cref="SocialLinksDto"/>.
    /// </summary>
    /// <param name="model">The social links domain model.</param>
    /// <returns>A new instance of <see cref="SocialLinksDto"/>.</returns>
    public static SocialLinksDto ToDto(this SocialLinks model)
        => new()
        {
            LinkedIn = model.LinkedIn?.Value,
            GitHub = model.GitHub?.Value,
            Twitter = model.Twitter?.Value,
            Telegram = model.Telegram?.Value
        };

    /// <summary>
    /// Converts a <see cref="SocialLinksDto"/> to a <see cref="SocialLinks"/> domain model.
    /// </summary>
    /// <param name="dto">The social links data transfer object.</param>
    /// <returns>A new instance of <see cref="SocialLinks"/> domain model.</returns>
    public static SocialLinks ToModel(this SocialLinksDto dto)
        => SocialLinks.Create(dto.LinkedIn, dto.GitHub, dto.Twitter, dto.Telegram);

    /// <summary>
    /// Converts a <see cref="ProjectItem"/> domain model to a <see cref="ProjectDto"/>.
    /// </summary>
    /// <param name="model">The project domain model.</param>
    /// <returns>A new instance of <see cref="ProjectDto"/>.</returns>
    public static ProjectDto ToDto(this ProjectItem model)
        => new()
        {
            Id = model.Id.Value,
            Name = model.Name.Value,
            Description = model.Description?.Value,
            IconUrl = model.Icon?.ImageUrl.Value,
            LinkUrl = model.Link.Value?.Value,
            TechStack = model.TechStack.Select(tag => tag.Value).ToArray()
        };

    /// <summary>
    /// Converts a collection of <see cref="ProjectItem"/> domain models to an array of <see cref="ProjectDto"/>.
    /// </summary>
    /// <param name="models">The collection of project domain models.</param>
    /// <returns>An array of <see cref="ProjectDto"/>.</returns>
    public static ProjectDto[] ToDto(this IEnumerable<ProjectItem> models)
        => models.Select(item => item.ToDto()).ToArray();

    /// <summary>
    /// Converts a <see cref="ProjectDto"/> to a <see cref="ProjectItem"/> domain model.
    /// </summary>
    /// <param name="dto">The project data transfer object.</param>
    /// <returns>A new instance of <see cref="ProjectItem"/> domain model.</returns>
    public static ProjectItem ToModel(this ProjectDto dto) => ProjectItem.Create(
        new ProjectId(dto.Id ?? Guid.NewGuid()),
        new ProjectName(dto.Name ?? string.Empty),
        ProjectDescription.TryFrom(dto.Description),
        ProjectIcon.TryFrom(dto.IconUrl),
        ProjectLink.From(dto.LinkUrl),
        dto.TechStack?.Select(value => new TechTag(value)) ?? []
    );

    /// <summary>
    /// Converts a <see cref="WorkExperienceItem"/> domain model to a <see cref="WorkExperienceDto"/>.
    /// </summary>
    /// <param name="model">The work experience domain model.</param>
    /// <returns>A new instance of <see cref="WorkExperienceDto"/>.</returns>
    public static WorkExperienceDto ToDto(this WorkExperienceItem model) => new()
    {
        Id = model.Id.Value,
        Company = model.Company.Value,
        Location = model.Location?.ToDto(),
        Role = model.Role?.Value ?? string.Empty,
        Description = model.Description?.Value,
        StartDate = model.Period.Start,
        EndDate = model.Period.End,
        TechStack = model.TechStack.Select(tag => tag.Value).ToArray()
    };

    /// <summary>
    /// Converts a collection of <see cref="WorkExperienceItem"/> domain models to an array of <see cref="WorkExperienceDto"/>.
    /// </summary>
    /// <param name="models">The collection of work experience domain models.</param>
    /// <returns>An array of <see cref="WorkExperienceDto"/>.</returns>
    public static WorkExperienceDto[] ToDto(this IEnumerable<WorkExperienceItem> models)
        => models.Select(item => item.ToDto()).ToArray();

    /// <summary>
    /// Converts a <see cref="WorkExperienceDto"/> to a <see cref="WorkExperienceItem"/> domain model.
    /// </summary>
    /// <param name="dto">The work experience data transfer object.</param>
    /// <returns>A new instance of <see cref="WorkExperienceItem"/> domain model.</returns>
    public static WorkExperienceItem ToModel(this WorkExperienceDto dto)
        => WorkExperienceItem.Create(
            new WorkExperienceId(dto.Id ?? Guid.NewGuid()),
            new CompanyName(dto.Company ?? string.Empty),
            dto.Location?.ToModel(),
            new RoleTitle(dto.Role ?? string.Empty),
            WorkDescription.TryFrom(dto.Description),
            DateRange.From(dto.StartDate ?? DateOnly.MinValue, dto.EndDate),
            dto.TechStack?.Select(value => new TechTag(value)) ?? []
        );

    /// <summary>
    /// Converts a <see cref="DeveloperProfile"/> domain model to a <see cref="DeveloperProfileDto"/>.
    /// </summary>
    /// <param name="model">The developer profile domain model.</param>
    /// <returns>A new instance of <see cref="DeveloperProfileDto"/>.</returns>
    public static DeveloperProfileDto ToDto(this DeveloperProfile model)
        => new()
        {
            Id = model.Id.Value,
            FirstName = model.Name.FirstName,
            LastName = model.Name.LastName,
            Email = model.Contact.Email.Value,
            Phone = model.Contact.Phone?.Value,
            Website = model.Contact.Website?.Value,
            Role = model.Role?.Value,
            Summary = model.Summary?.Value,
            AvatarUrl = model.Avatar?.ImageUrl.Value,
            OpenToWork = model.OpenToWork.Value,
            Verified = model.Verification.Value.ToString(),
            YearsOfExperience = model.YearsOfExperience.Value,
            Location = model.Contact.Location?.ToDto(),
            SocialLinks = model.Social.ToDto(),
            Skills = model.Skills.Select(tag => tag.Value).ToArray(),
            Projects = model.Projects.ToDto(),
            WorkExperience = model.WorkExperience.ToDto()
        };

    /// <summary>
    /// Converts a collection of <see cref="DeveloperProfile"/> domain models to an array of <see cref="DeveloperProfileDto"/>.
    /// </summary>
    /// <param name="models">The collection of developer profile domain models.</param>
    /// <returns>An array of <see cref="DeveloperProfileDto"/>.</returns>
    public static DeveloperProfileDto[] ToDto(this IEnumerable<DeveloperProfile> models)
        => models.Select(profile => profile.ToDto()).ToArray();

    /// <summary>
    /// Converts a <see cref="DeveloperProfileDto"/> to a <see cref="DeveloperProfile"/> domain model.
    /// </summary>
    /// <param name="dto">The developer profile data transfer object.</param>
    /// <param name="name">The developer's name.</param>
    /// <param name="email">The developer's email.</param>
    /// <param name="website">The developer's website.</param>
    /// <param name="now">The current timestamp.</param>
    /// <returns>A new instance of <see cref="DeveloperProfile"/> domain model.</returns>
    public static DeveloperProfile ToModel(this DeveloperProfileDto dto, PersonName name, EmailAddress email, Url? website, DateTimeOffset now)
        => DeveloperProfile.FromPersistence(
            new DeveloperId(dto.Id),
            name,
            RoleTitle.TryFrom(dto.Role),
            ProfileSummary.TryFrom(dto.Summary),
            Avatar.TryFrom(dto.AvatarUrl),
            ContactInfo.Create(dto.Location?.ToModel(), email, PhoneNumber.TryFrom(dto.Phone), website),
            dto.SocialLinks?.ToModel() ?? SocialLinks.Create(null, null, null, null),
            new VerificationStatus(Enum.Parse<VerificationLevel>(dto.Verified ?? nameof(VerificationLevel.NotVerified), true)),
            new OpenToWorkStatus(dto.OpenToWork),
            YearsOfExperience.From(dto.YearsOfExperience),
            dto.Skills.Select(value => new SkillTag(value)),
            dto.Projects.Select(projectDto => projectDto.ToModel()),
            dto.WorkExperience.Select(experienceDto => experienceDto.ToModel()),
            now,
            now
        );
}