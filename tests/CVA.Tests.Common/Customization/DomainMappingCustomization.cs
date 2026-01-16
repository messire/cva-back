namespace CVA.Tests.Common;

/// <summary>
/// Customization for domain-specific models and DTOs.
/// </summary>
public sealed class DomainMappingCustomization : ICustomization
{
    /// <inheritdoc />
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(EmailAddressBuilder.Instance);
        fixture.Customizations.Add(PhoneNumberBuilder.Instance);
        fixture.Customizations.Add(UrlBuilder.Instance);
        fixture.Customizations.Add(YearsOfExperienceBuilder.Instance);
        fixture.Customizations.Add(SocialLinksDtoBuilder.Instance);
        fixture.Customizations.Add(LocationDtoBuilder.Instance);
        fixture.Customizations.Add(UserBuilder.Instance);
        fixture.Customizations.Add(DeveloperProfileBuilder.Instance);
        fixture.Customizations.Add(ProjectItemBuilder.Instance);
        fixture.Customizations.Add(WorkExperienceItemBuilder.Instance);
    }
}