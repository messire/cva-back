namespace CVA.Tests.Common;

/// <summary>
/// Base <see cref="AutoFixture"/> customization.
/// </summary>
public sealed class BaseFixtureCustomization : ICustomization
{
    /// <summary>
    /// Customizes the AutoFixture instance by replacing the default recursion behavior
    /// and adding custom builders.
    /// </summary>
    /// <param name="fixture">The fixture to customize.</param>
    public void Customize(IFixture fixture)
    {
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(Remove(fixture));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        fixture.Customizations.Add(DateTimeBuilder.Instance);
        fixture.Customizations.Add(DateOnlyBuilder.Instance);
    }

    private static Action<ThrowingRecursionBehavior> Remove(IFixture fixture)
        => behavior => fixture.Behaviors.Remove(behavior);
}
