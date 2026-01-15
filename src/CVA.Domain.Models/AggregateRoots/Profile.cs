namespace CVA.Domain.Models;

public sealed partial class DeveloperProfile
{
    /// <summary>
    /// Changes the name of the developer profile to the specified value and updates the last modified timestamp.
    /// </summary>
    /// <param name="name">The new name for the developer profile.</param>
    /// <param name="now">The timestamp indicating when the name change occurred.</param>
    public void ChangeName(PersonName name, DateTimeOffset now)
    {
        Ensure.NotNull(name, nameof(name));
        Name = name;
        Touch(now);
    }

    /// <summary>
    /// Updates the role of the developer profile to the specified value and sets the last modified timestamp.
    /// </summary>
    /// <param name="role">The new role for the developer profile.</param>
    /// <param name="now">The timestamp indicating when the role change occurred.</param>
    public void ChangeRole(RoleTitle? role, DateTimeOffset now)
    {
        Role = role;
        Touch(now);
    }

    /// <summary>
    /// Changes the summary of the developer profile to the specified value and updates the last modified timestamp.
    /// </summary>
    /// <param name="summary">The new summary for the developer profile.</param>
    /// <param name="now">The timestamp indicating when the summary change occurred.</param>
    public void ChangeSummary(ProfileSummary? summary, DateTimeOffset now)
    {
        Summary = summary;
        Touch(now);
    }

    /// <summary>
    /// Changes the avatar of the developer profile to the specified value and updates the last modified timestamp.
    /// </summary>
    /// <param name="avatar">The new avatar for the developer profile.</param>
    /// <param name="now">The timestamp indicating when the avatar change occurred.</param>
    public void ChangeAvatar(Avatar? avatar, DateTimeOffset now)
    {
        Avatar = avatar;
        Touch(now);
    }

    /// <summary>
    /// Sets the open-to-work status of the developer profile to the specified value and updates the last modified timestamp.
    /// </summary>
    /// <param name="value">The new open-to-work status for the developer profile.</param>
    /// <param name="now">The timestamp indicating when the open-to-work status change occurred.</param>
    public void SetOpenToWork(bool value, DateTimeOffset now)
    {
        OpenToWork = new OpenToWorkStatus(value);
        Touch(now);
    }


    /// <summary>
    /// Changes the contact information of the developer profile to the specified values and updates the last modified timestamp.
    /// </summary>
    /// <param name="contact">The new contact information for the developer profile.</param>
    /// <param name="now">The timestamp indicating when the contact information change occurred.</param>
    public void ChangeContact(ContactInfo contact, DateTimeOffset now)
    {
        Ensure.NotNull(contact, nameof(contact));
        Contact = contact;
        Touch(now);
    }

    /// <summary>
    /// Changes the social links of the developer profile to the specified values and updates the last modified timestamp.
    /// </summary>
    /// <param name="social">The new social links for the developer profile.</param>
    /// <param name="now">The timestamp indicating when the social links change occurred.</param>
    public void ChangeSocialLinks(SocialLinks social, DateTimeOffset now)
    {
        Ensure.NotNull(social, nameof(social));
        Social = social;
        Touch(now);
    }
    /// <summary>
    /// Sets the verification status of the developer profile to the specified value and updates the last modified timestamp.
    /// </summary>
    /// <param name="value">The new verification status for the developer profile.</param>
    /// <param name="now">The timestamp indicating when the verification status change occurred.</param>
    public void SetVerified(VerificationStatus value, DateTimeOffset now)
    {
        Ensure.NotNull(value, nameof(value));
        Verification = value;
        Touch(now);
    }
}