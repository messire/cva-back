namespace CVA.Domain.Models;

public sealed partial class DeveloperProfile
{
    /// <summary>
    /// Creates a new instance of the <see cref="DeveloperProfile"/> class from persistence layer data.
    /// </summary>
    /// <param name="id">The unique identifier for the developer profile.</param>
    /// <param name="name">The name of the developer.</param>
    /// <param name="role">The role or title of the developer.</param>
    /// <param name="summary">A summary of the developer's profile.</param>
    /// <param name="avatar">The avatar or profile picture of the developer.</param>
    /// <param name="openToWork">The developer's availability for new work opportunities.</param>
    /// <param name="verification">The verification status of the developer's profile.</param>
    /// <param name="contact">The developer's contact information.</param>
    /// <param name="social">The developer's social media links or handles.</param>
    /// <param name="skills">A collection of skill tags associated with the developer.</param>
    /// <param name="projects">A collection of the developer's project items.</param>
    /// <param name="workExperience">A collection of the developer's work experience items.</param>
    /// <param name="createdAt">The date and time when the profile was created.</param>
    /// <param name="updatedAt">The date and time when the profile was last updated.</param>
    /// <returns>A new instance of the <see cref="DeveloperProfile"/> class populated with the provided data.</returns>
    public static DeveloperProfile FromPersistence(
        DeveloperId id,
        PersonName name,
        RoleTitle? role,
        ProfileSummary? summary,
        Avatar? avatar,
        ContactInfo contact,
        SocialLinks social,
        VerificationStatus verification,
        OpenToWorkStatus openToWork,
        IEnumerable<SkillTag> skills,
        IEnumerable<ProjectItem> projects,
        IEnumerable<WorkExperienceItem> workExperience,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt)
    {
        var profile = new DeveloperProfile(id, name, role, summary, avatar, openToWork, contact, social, verification, createdAt, updatedAt);

        profile._skills.AddRange(skills);
        profile._projects.AddRange(projects);
        profile._workExperience.AddRange(workExperience);

        return profile;
    }
}