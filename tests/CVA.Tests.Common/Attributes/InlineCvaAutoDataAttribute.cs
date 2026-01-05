using AutoFixture.Xunit2;

namespace CVA.Tests.Common.Attributes;

/// <summary>
/// Provides a set of data for a Theory, allowing to mix manual data and auto-generated data.
/// </summary>
public sealed class InlineCvaAutoDataAttribute(params object[]? values) : InlineAutoDataAttribute(new CvaAutoDataAttribute(), values);