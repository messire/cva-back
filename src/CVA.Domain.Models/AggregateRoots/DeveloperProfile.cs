namespace CVA.Domain.Models;

/// <summary>
/// Represents a developer profile.
/// </summary>
public sealed partial class DeveloperProfile: AggregateRoot
{
    private readonly List<SkillTag> _skills = new();
    private readonly List<ProjectItem> _projects = new();
    private readonly List<WorkExperienceItem> _workExperience = new();

    /// <summary>
    /// The unique identifier of the developer.
    /// </summary>
    public DeveloperId Id { get; }

    /// <summary>
    /// The name of the developer.
    /// </summary>
    public PersonName Name { get; private set; }

    /// <summary>
    /// The role of the developer.
    /// </summary>
    public RoleTitle? Role { get; private set; }

    /// <summary>
    /// The summary of the developer's profile.
    /// </summary>
    public ProfileSummary? Summary { get; private set; }

    /// <summary>
    /// The avatar image of the developer.
    /// </summary>
    public Avatar? Avatar { get; private set; }

    /// <summary>
    /// The status indicating if the developer is open to work.
    /// </summary>
    public OpenToWorkStatus OpenToWork { get; private set; }

    /// <summary>
    /// The years of experience of the developer.
    /// </summary>
    public YearsOfExperience YearsOfExperience { get; private set; }

    /// <summary>
    /// The contact information of the developer.
    /// </summary>
    public ContactInfo Contact { get; private set; }

    /// <summary>
    /// The social media links of the developer.
    /// </summary>
    public SocialLinks Social { get; private set; }

    /// <summary>
    /// The verification status of the developer profile.
    /// </summary>
    public VerificationStatus Verification { get; private set; }

    /// <summary>
    /// The list of skills possessed by the developer.
    /// </summary>
    public IReadOnlyList<SkillTag> Skills => _skills;

    /// <summary>
    /// The list of projects the developer has worked on.
    /// </summary>
    public IReadOnlyList<ProjectItem> Projects => _projects;

    /// <summary>
    /// The list of work experiences the developer has had.
    /// </summary>
    public IReadOnlyList<WorkExperienceItem> WorkExperience => _workExperience;

    /// <summary>
    /// Private constructor.
    /// </summary>
    /// <param name="id"> Developer identifier. </param>
    /// <param name="name"> Developer's full name. </param>
    /// <param name="role"> Developer's role title. </param>
    /// <param name="summary"> Developer's profile summary. </param>
    /// <param name="avatar"> Developer's avatar image. </param>
    /// <param name="openToWork"> Developer's open to work status. </param>
    /// <param name="yearsOfExperience"> Developer's years of experience. </param>
    /// <param name="contact"> Developer's contact information. </param>
    /// <param name="social"> Developer's social links. </param>
    /// <param name="verification"> Developer's verification status. </param>
    /// <param name="createdAt"> Developer's profile creation timestamp. </param>
    /// <param name="updatedAt"> Developer's profile last update timestamp. </param>
    private DeveloperProfile(
        DeveloperId id,
        PersonName name,
        RoleTitle? role,
        ProfileSummary? summary,
        Avatar? avatar,
        OpenToWorkStatus openToWork,
        YearsOfExperience yearsOfExperience,
        ContactInfo contact,
        SocialLinks social,
        VerificationStatus verification,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt)
    {
        Id = id;
        Name = name;
        Role = role;
        Summary = summary;
        Avatar = avatar;
        OpenToWork = openToWork;
        YearsOfExperience = yearsOfExperience;
        Contact = contact;
        Social = social;
        Verification = verification;
        Touch(updatedAt);
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Creates a new instance of the DeveloperProfile class with the specified details.
    /// </summary>
    /// <param name="id">Unique identifier for the developer.</param>
    /// <param name="name">The developer's full name.</param>
    /// <param name="role">The developer's role or title.</param>
    /// <param name="summary">A summary of the developer's profile.</param>
    /// <param name="avatar">The developer's avatar image.</param>
    /// <param name="contact">The developer's contact information.</param>
    /// <param name="social">The developer's social media links.</param>
    /// <param name="verification">Verification status of the developer's profile.</param>
    /// <param name="openToWork">The developer's open-to-work status.</param>
    /// <param name="yearsOfExperience">The number of years of experience the developer has.</param>
    /// <param name="now">The current timestamp to assign as the creation and last update time.</param>
    /// <returns>A new instance of the DeveloperProfile class.</returns>
    public static DeveloperProfile Create(
        DeveloperId id,
        PersonName name,
        RoleTitle? role,
        ProfileSummary? summary,
        Avatar? avatar,
        ContactInfo contact,
        SocialLinks social,
        VerificationStatus verification,
        OpenToWorkStatus openToWork,
        YearsOfExperience yearsOfExperience,
        DateTimeOffset now)
    {
        Ensure.NotEmpty(id.Value, nameof(id));
        Ensure.NotNull(name, nameof(name));
        Ensure.NotNull(contact, nameof(contact));
        Ensure.NotNull(social, nameof(social));
        Ensure.NotNull(verification, nameof(verification));
        Ensure.NotNull(openToWork, nameof(openToWork));
        Ensure.NotNull(yearsOfExperience, nameof(yearsOfExperience));

        return new DeveloperProfile(id, name, role, summary, avatar, openToWork, yearsOfExperience, contact, social, verification, createdAt: now, updatedAt: now);
    }
}