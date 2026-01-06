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
            YearsOfExperience = profile.YearsOfExperience.Value,
            Email = profile.Contact.Email.Value,
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

    extension(DeveloperProfileEntity entity)
    {
        public void UpdateFromDomain(DeveloperProfile profile)
        {
            entity.FirstName = profile.Name.FirstName;
            entity.LastName = profile.Name.LastName;
            entity.Role = profile.Role?.Value;
            entity.Summary = profile.Summary?.Value;
            entity.AvatarUrl = profile.Avatar?.ImageUrl.Value;
            entity.OpenToWork = profile.OpenToWork.Value;
            entity.YearsOfExperience = profile.YearsOfExperience.Value;
            entity.Email = profile.Contact.Email.Value;
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

            entity.Projects.Clear();
            entity.Projects.AddRange(profile.Projects.Select(item => item.ToEntity(entity.Id)));

            entity.WorkExperience.Clear();
            entity.WorkExperience.AddRange(profile.WorkExperience.Select(item => item.ToEntity(entity.Id)));

            entity.UpdatedAt = profile.UpdatedAt;
        }

        public DeveloperProfile ToDomain()
        {
            var id = new DeveloperId(entity.Id);

            var contactInfo = ContactInfo.Create(
                Location.TryFrom(entity.Location?.City, entity.Location?.Country),
                EmailAddress.From(entity.Email),
                Url.TryFrom(entity.Website));

            var socialsEntity = entity.SocialLinks;
            var socialLinks = SocialLinks.Create(socialsEntity?.LinkedIn, socialsEntity?.GitHub, socialsEntity?.Twitter, socialsEntity?.Telegram);

            var skills = entity.Skills.Select(SkillTag.From).ToArray();

            var projects = entity.Projects.Select(p =>
                    ProjectItem.FromPersistence(
                        new ProjectId(p.Id),
                        ProjectName.From(p.Name),
                        ProjectDescription.TryFrom(p.Description),
                        ProjectIcon.TryFrom(p.IconUrl),
                        ProjectLink.From(p.LinkUrl),
                        p.TechStack.Select(TechTag.From)))
                .ToArray();

            var work = entity.WorkExperience.Select(w =>
                    WorkExperienceItem.FromPersistence(
                        new WorkExperienceId(w.Id),
                        CompanyName.From(w.Company),
                        Location.TryFrom(w.Location?.City, w.Location?.Country),
                        RoleTitle.From(w.Role),
                        WorkDescription.TryFrom(w.Description),
                        DateRange.From(w.StartDate, w.EndDate),
                        w.TechStack.Select(TechTag.From)))
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
                yearsOfExperience: YearsOfExperience.From(entity.YearsOfExperience),
                skills: skills,
                projects: projects,
                workExperience: work,
                createdAt: entity.CreatedAt,
                updatedAt: entity.UpdatedAt);
        }
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
            Role = work.Role.Value,
            Description = work.Description?.Value,
            StartDate = work.Period.Start,
            EndDate = work.Period.End,
            TechStack = work.TechStack.Select(tag => tag.Value).ToList()
        };
}