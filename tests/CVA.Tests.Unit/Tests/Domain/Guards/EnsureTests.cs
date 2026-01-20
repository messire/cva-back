using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.Guards;

/// <summary>
/// Unit tests for the <see cref="Ensure"/> guard class.
/// </summary>
[Trait(Layer.Domain, Category.Guards)]
public class EnsureTests
{
    /// <summary>
    /// Purpose: Verify NotNull guard.
    /// Should: Not throw exception when value is not null.
    /// When: A valid object is provided.
    /// </summary>
    [Fact]
    public void NotNull_Should_Not_Throw_When_Value_Is_Not_Null()
    {
        // Arrange
        var value = new object();

        // Act
        var exception = Record.Exception(() => Ensure.NotNull(value, nameof(value)));

        // Assert
        Assert.Null(exception);
    }

    /// <summary>
    /// Purpose: Verify NotNull guard.
    /// Should: Throw ArgumentNullException when value is null.
    /// When: null is provided.
    /// </summary>
    [Fact]
    public void NotNull_Should_Throw_ArgumentNullException_When_Value_Is_Null()
    {
        // Arrange
        object? value = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Ensure.NotNull(value, "paramName"));
        Assert.Equal("paramName", exception.ParamName);
    }

    /// <summary>
    /// Purpose: Verify NotEmpty guard for Guid.
    /// Should: Not throw exception when Guid is not empty.
    /// When: A non-empty Guid is provided.
    /// </summary>
    [Fact]
    public void NotEmpty_Should_Not_Throw_When_Guid_Is_Not_Empty()
    {
        // Arrange
        var value = Guid.NewGuid();

        // Act
        var exception = Record.Exception(() => Ensure.NotEmpty(value, nameof(value)));

        // Assert
        Assert.Null(exception);
    }

    /// <summary>
    /// Purpose: Verify NotEmpty guard for Guid.
    /// Should: Throw ArgumentException when Guid is empty.
    /// When: Guid.Empty is provided.
    /// </summary>
    [Fact]
    public void NotEmpty_Should_Throw_ArgumentException_When_Guid_Is_Empty()
    {
        // Arrange
        var value = Guid.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Ensure.NotEmpty(value, "paramName"));
        Assert.Equal("paramName", exception.ParamName);
        Assert.Contains("Empty guid.", exception.Message);
    }

    /// <summary>
    /// Purpose: Verify TrimToNull guard.
    /// Should: Return trimmed string when valid string is provided.
    /// When: String with spaces is provided.
    /// </summary>
    [Theory]
    [InlineData("  test  ", "test")]
    [InlineData("test", "test")]
    public void TrimToNull_Should_Return_Trimmed_String(string input, string expected)
    {
        // Act
        var result = Ensure.TrimToNull(input, "paramName");

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Purpose: Verify TrimToNull guard.
    /// Should: Throw ArgumentException when string is null, empty or whitespace.
    /// When: null, empty or whitespace string is provided.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void TrimToNull_Should_Throw_ArgumentException_When_Value_Is_Invalid(string? input)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Ensure.TrimToNull(input, "paramName"));
        Assert.Equal("paramName", exception.ParamName);
        Assert.Contains("Empty value.", exception.Message);
    }
}
