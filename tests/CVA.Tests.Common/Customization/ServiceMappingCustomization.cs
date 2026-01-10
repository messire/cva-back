using CVA.Application.IdentityService;

namespace CVA.Tests.Common;

/// <summary>
/// Customization for mapping service interfaces to their implementations.
/// </summary>
public class ServiceMappingCustomization: ICustomization
{
    /// <inheritdoc />
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(IIdentityService), typeof(IdentityService)));
    }
}