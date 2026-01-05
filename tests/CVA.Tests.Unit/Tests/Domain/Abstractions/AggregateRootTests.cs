using CVA.Domain.Models;
using CVA.Tests.Unit.Stubs;

namespace CVA.Tests.Unit.Domain.Abstractions;

/// <summary>
/// Unit tests for the <see cref="AggregateRoot"/> base class.
/// </summary>
[Trait(Layer.Domain, Category.Abstractions)]
public class AggregateRootTests
{
    /// <summary>
    /// Purpose: Ensure that ReplaceList throws when source collection contains null elements.
    /// Should: Throw DomainValidationException with appropriate message.
    /// When: Collection with null element is passed.
    /// </summary>
    [Fact]
    public void ReplaceList_Should_Throw_When_Source_Contains_Null()
    {
        // Arrange
        var target = new List<string>();
        var source = new[] { "A", null! };

        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() => NullAggregate.ReplaceList(target, source));
        Assert.Equal("Collection must not contain null elements.", exception.Message);
    }

    /// <summary>
    /// Purpose: Verify that ReplaceList clears the target collection when source is null.
    /// Should: Clear the target list and return.
    /// When: null source is provided.
    /// </summary>
    [Fact]
    public void ReplaceList_Should_Clear_Target_When_Source_Is_Null()
    {
        // Arrange
        var target = new List<string> { "Existing" };

        // Act
        NullAggregate.ReplaceList(target, null);

        // Assert
        Assert.Empty(target);
    }
}
