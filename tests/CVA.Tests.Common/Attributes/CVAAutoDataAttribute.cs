using AutoFixture.Xunit2;

namespace CVA.Tests.Common.Attributes;

/// <summary>
/// AutoData attribute for unit tests.
/// </summary>
public sealed class CvaAutoDataAttribute() : AutoDataAttribute(() => new Fixture().Customize(new ApplicationTestCustomization()));