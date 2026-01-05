using AutoFixture.AutoMoq;

namespace CVA.Tests.Common;

/// <summary>
/// General customization for application layer unit tests.
/// Integrates Moq and custom specimen builders.
/// </summary>
public sealed class ApplicationTestCustomization : CompositeCustomization
{
    private static readonly ICustomization[] CustomizationCollection =
    [
        new AutoMoqCustomization { ConfigureMembers = true },
        new BaseFixtureCustomization(),
        new DomainMappingCustomization(),
        new ServiceMappingCustomization()
    ];

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationTestCustomization"/> class.
    /// </summary>
    public ApplicationTestCustomization()
        : base(CustomizationCollection)
    {
    }
}