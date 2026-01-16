namespace CVA.Tests.Common;

/// <summary>
/// A builder class that creates instances of the <see cref="DeveloperProfile"/> type using the AutoFixture library.
/// </summary>
internal sealed class DeveloperProfileBuilder : ISpecimenBuilder
{
    /// <summary>
    /// A singleton instance of the <see cref="DeveloperProfileBuilder"/> class.
    /// </summary>
    public static readonly ISpecimenBuilder Instance = new DeveloperProfileBuilder();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(DeveloperProfile)) return new NoSpecimen();

        return DeveloperProfile.FromPersistence(
            id: (DeveloperId)context.Resolve(typeof(DeveloperId)),
            name: (PersonName)context.Resolve(typeof(PersonName)),
            role: (RoleTitle)context.Resolve(typeof(RoleTitle)),
            summary: (ProfileSummary)context.Resolve(typeof(ProfileSummary)),
            avatar: (Avatar)context.Resolve(typeof(Avatar)),
            contact: (ContactInfo)context.Resolve(typeof(ContactInfo)),
            social: (SocialLinks)context.Resolve(typeof(SocialLinks)),
            verification: (VerificationStatus)context.Resolve(typeof(VerificationStatus)),
            openToWork: (OpenToWorkStatus)context.Resolve(typeof(OpenToWorkStatus)),
            skills: (IEnumerable<SkillTag>)context.Resolve(typeof(IEnumerable<SkillTag>)),
            projects: (IEnumerable<ProjectItem>)context.Resolve(typeof(IEnumerable<ProjectItem>)),
            workExperience: (IEnumerable<WorkExperienceItem>)context.Resolve(typeof(IEnumerable<WorkExperienceItem>)),
            createdAt: (DateTimeOffset)context.Resolve(typeof(DateTimeOffset)),
            updatedAt: (DateTimeOffset)context.Resolve(typeof(DateTimeOffset))
        );
    }
}