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
        fixture.Customizations.Add(UrlBuilder.Instance);
        fixture.Customizations.Add(YearsOfExperienceBuilder.Instance);
        fixture.Customizations.Add(WorkDtoBuilder.Instance);
        fixture.Customizations.Add(UserDtoBuilder.Instance);
        fixture.Customizations.Add(UserBuilder.Instance);
        fixture.Customizations.Add(WorkBuilder.Instance);
    }
}