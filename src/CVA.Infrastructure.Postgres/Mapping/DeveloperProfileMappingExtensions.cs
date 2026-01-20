namespace CVA.Infrastructure.Postgres;

internal static class DeveloperProfileMappingExtensions
{
    public static DeveloperProfileEntity ToEntity(this DeveloperProfile profile)
    {
        var entity = new DeveloperProfileEntity
        {
            Id = profile.Id.Value,
            FirstName = profile.Name.FirstName,
            LastName = profile.Name.LastName,
            Role = profile.Role?.Value,
            Summary = profile.Summary?.Value,
            AvatarUrl = profile.Avatar?.ImageUrl.Value,
            OpenToWork = profile.OpenToWork.Value,
            Email = profile.Contact.Email.Value,
            Phone = profile.Contact.Phone?.Value,
            Website = profile.Contact.Website?.Value,
            Location = profile.Contact.Location.ToEntity(),
            SocialLinks = profile.Social.ToEntity(),
            Skills = profile.Skills.Select(tag => tag.Value).ToList(),
            Verified = profile.Verification.Value,
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt
        };

        entity.Projects.AddRange(profile.Projects.Select(item => item.ToEntity(profile.Id.Value)));
        entity.WorkExperience.AddRange(profile.WorkExperience.Select(item => item.ToEntity(profile.Id.Value)));

        return entity;
    }

    public static void UpdateFromDomain(this DeveloperProfileEntity entity, DeveloperProfile profile)
    {
        entity.FirstName = profile.Name.FirstName;
        entity.LastName = profile.Name.LastName;
        entity.Role = profile.Role?.Value;
        entity.Summary = profile.Summary?.Value;
        entity.AvatarUrl = profile.Avatar?.ImageUrl.Value;
        entity.OpenToWork = profile.OpenToWork.Value;
        entity.Email = profile.Contact.Email.Value;
        entity.Phone = profile.Contact.Phone?.Value;
        entity.Website = profile.Contact.Website?.Value;
        entity.Location = profile.Contact.Location.ToEntity();
        entity.SocialLinks ??= new SocialLinksEntity();
        entity.SocialLinks.LinkedIn = profile.Social.LinkedIn?.Value;
        entity.SocialLinks.GitHub = profile.Social.GitHub?.Value;
        entity.SocialLinks.Twitter = profile.Social.Twitter?.Value;
        entity.SocialLinks.Telegram = profile.Social.Telegram?.Value;
        entity.Verified = profile.Verification.Value;

        entity.Skills.Clear();
        entity.Skills.AddRange(profile.Skills.Select(tag => tag.Value));

        var projectIds = profile.Projects.Select(item => item.Id.Value).ToHashSet();
        entity.Projects.RemoveAll(projectEntity => !projectIds.Contains(projectEntity.Id));
        foreach (var project in profile.Projects)
        {
            var existing = entity.Projects.FirstOrDefault(projectEntity => projectEntity.Id == project.Id.Value);
            if (existing is not null)
            {
                existing.UpdateFromDomain(project);
            }
            else
            {
                entity.Projects.Add(project.ToEntity(entity.Id));
            }
        }

        var workExperienceIds = profile.WorkExperience.Select(w => w.Id.Value).ToHashSet();
        entity.WorkExperience.RemoveAll(experienceEntity => !workExperienceIds.Contains(experienceEntity.Id));
        foreach (var work in profile.WorkExperience)
        {
            var existing = entity.WorkExperience.FirstOrDefault(experienceEntity => experienceEntity.Id == work.Id.Value);
            if (existing is not null)
            {
                existing.UpdateFromDomain(work);
            }
            else
            {
                entity.WorkExperience.Add(work.ToEntity(entity.Id));
            }
        }

        entity.UpdatedAt = profile.UpdatedAt;
    }

    public static DeveloperProfile ToDomain(this DeveloperProfileEntity entity)
    {
        var id = new DeveloperId(entity.Id);

        var contactInfo = ContactInfo.Create(
            Location.TryFrom(entity.Location?.City, entity.Location?.Country),
            EmailAddress.From(entity.Email),
            PhoneNumber.TryFrom(entity.Phone),
            Url.TryFrom(entity.Website));

        var socialsEntity = entity.SocialLinks;
        var socialLinks = SocialLinks.Create(socialsEntity?.LinkedIn, socialsEntity?.GitHub, socialsEntity?.Twitter, socialsEntity?.Telegram);

        var skills = entity.Skills.Select(SkillTag.From).ToArray();

        var projects = entity.Projects.Select(projectEntity =>
                ProjectItem.FromPersistence(
                    new ProjectId(projectEntity.Id),
                    ProjectName.From(projectEntity.Name),
                    ProjectDescription.TryFrom(projectEntity.Description),
                    ProjectIcon.TryFrom(projectEntity.IconUrl),
                    ProjectLink.From(projectEntity.LinkUrl),
                    projectEntity.TechStack.Select(TechTag.From)))
            .ToArray();

        var work = entity.WorkExperience.Select(experienceEntity =>
                WorkExperienceItem.FromPersistence(
                    new WorkExperienceId(experienceEntity.Id),
                    CompanyName.From(experienceEntity.Company),
                    Location.TryFrom(experienceEntity.Location?.City, experienceEntity.Location?.Country),
                    RoleTitle.From(experienceEntity.Role),
                    WorkDescription.TryFrom(experienceEntity.Description),
                    DateRange.From(experienceEntity.StartDate, experienceEntity.EndDate),
                    experienceEntity.TechStack.Select(TechTag.From)))
            .ToArray();

        return DeveloperProfile.FromPersistence(
            id: id,
            name: PersonName.From(entity.FirstName, entity.LastName),
            role: RoleTitle.TryFrom(entity.Role),
            summary: ProfileSummary.TryFrom(entity.Summary),
            avatar: Avatar.TryFrom(entity.AvatarUrl),
            contact: contactInfo,
            social: socialLinks,
            verification: new VerificationStatus(entity.Verified),
            openToWork: new OpenToWorkStatus(entity.OpenToWork),
            skills: skills,
            projects: projects,
            workExperience: work,
            createdAt: entity.CreatedAt,
            updatedAt: entity.UpdatedAt);
    }

    private static void UpdateFromDomain(this ProjectEntity entity, ProjectItem project)
    {
        entity.Name = project.Name.Value;
        entity.Description = project.Description?.Value;
        entity.IconUrl = project.Icon?.ImageUrl.Value;
        entity.LinkUrl = project.Link.Value?.Value;
        entity.TechStack.Clear();
        entity.TechStack.AddRange(project.TechStack.Select(tag => tag.Value));
    }

    private static void UpdateFromDomain(this WorkExperienceEntity entity, WorkExperienceItem work)
    {
        entity.Company = work.Company.Value;
        entity.Location = work.Location.ToEntity();
        entity.Role = work.Role?.Value ?? string.Empty;
        entity.Description = work.Description?.Value;
        entity.StartDate = work.Period.Start;
        entity.EndDate = work.Period.End;
        entity.TechStack.Clear();
        entity.TechStack.AddRange(work.TechStack.Select(tag => tag.Value));
    }

    private static LocationEntity? ToEntity(this Location? location)
        => location is null ? null : new LocationEntity { City = location.City, Country = location.Country };

    private static SocialLinksEntity ToEntity(this SocialLinks links)
        => new()
        {
            LinkedIn = links.LinkedIn?.Value,
            GitHub = links.GitHub?.Value,
            Twitter = links.Twitter?.Value,
            Telegram = links.Telegram?.Value
        };

    private static ProjectEntity ToEntity(this ProjectItem project, Guid developerProfileId)
        => new()
        {
            Id = project.Id.Value,
            DeveloperProfileId = developerProfileId,
            Name = project.Name.Value,
            Description = project.Description?.Value,
            IconUrl = project.Icon?.ImageUrl.Value,
            LinkUrl = project.Link.Value?.Value,
            TechStack = project.TechStack.Select(tag => tag.Value).ToList()
        };

    private static WorkExperienceEntity ToEntity(this WorkExperienceItem work, Guid developerProfileId)
        => new()
        {
            Id = work.Id.Value,
            DeveloperProfileId = developerProfileId,
            Company = work.Company.Value,
            Location = work.Location.ToEntity(),
            Role = work.Role?.Value ?? string.Empty,
            Description = work.Description?.Value,
            StartDate = work.Period.Start,
            EndDate = work.Period.End,
            TechStack = work.TechStack.Select(tag => tag.Value).ToList()
        };
}