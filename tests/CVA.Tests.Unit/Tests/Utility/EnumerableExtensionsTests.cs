using CVA.Tools.Common;

namespace CVA.Tests.Unit.Utility;

/// <summary>
/// Unit tests for the <see cref="EnumerableExtensions"/> class.
/// </summary>
[Trait(Layer.Utility, Category.Helpers)]
public sealed class EnumerableExtensionsTests
{
    /// <summary>
    /// Purpose: Verify that ScrambledEquals returns true for integer collections with same elements in different order.
    /// Should: Return true regardless of element order.
    /// When: Two integer collections contain identical elements with identical frequencies.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(new[] { 1, 2, 3 }, new[] { 3, 2, 1 })]
    [InlineCvaAutoData(new[] { 1, 1, 2 }, new[] { 1, 2, 1 })]
    public void ScrambledEquals_Should_ReturnTrue_ForEquivalentIntCollections(int[] first, int[] second)
    {
        // Act
        var result = first.ScrambledEquals(second);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Purpose: Verify that ScrambledEquals returns true for string collections with same elements in different order.
    /// Should: Return true regardless of element order.
    /// When: Two string collections contain identical elements with identical frequencies.
    /// </summary>
    [Theory]
    [InlineCvaAutoData(new[] { "a", "b" }, new[] { "b", "a" })]
    public void ScrambledEquals_Should_ReturnTrue_ForEquivalentStringCollections(string[] first, string[] second)
    {
        // Act
        var result = first.ScrambledEquals(second);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Purpose: Ensure ScrambledEquals detects differences in element frequency.
    /// Should: Return false when element counts do not match.
    /// When: One collection has duplicate elements that the other does not.
    /// </summary>
    [Fact]
    public void ScrambledEquals_Should_ReturnFalse_WhenFrequenciesDiffer()
    {
        // Arrange
        var first = new[] { 1, 1, 2 };
        var second = new[] { 1, 2, 2 };

        // Act
        var result = first.ScrambledEquals(second);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Purpose: Verify that ForEach executes the action for every element.
    /// Should: Invoke the callback exactly N times.
    /// When: A non-empty collection is processed.
    /// </summary>
    [Theory, CvaAutoData]
    public void ForEach_Should_ExecuteActionForEachElement(List<int> items)
    {
        // Arrange
        var count = 0;

        // Act
        items.ForEach(_ => count++);

        // Assert
        Assert.Equal(items.Count, count);
    }
}