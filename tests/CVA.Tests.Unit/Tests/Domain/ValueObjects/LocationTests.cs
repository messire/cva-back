using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="Location"/> value object.
/// </summary>
[Trait(Layer.Domain, Category.ValueObjects)]
public class LocationTests
{
    /// <summary>
    /// Purpose: Verify that From method correctly trims city and country.
    /// Should: Trim whitespace from both values.
    /// When: City and country strings with extra spaces are provided.
    /// </summary>
    [Theory]
    [InlineData("Moscow", "Russia", "Moscow", "Russia")]
    [InlineData("  New York  ", "  USA  ", "New York", "USA")]
    public void From_Should_Trim_Values(string city, string country, string expectedCity, string expectedCountry)
    {
        // Act
        var location = Location.From(city, country);

        // Assert
        Assert.Equal(expectedCity, location.City);
        Assert.Equal(expectedCountry, location.Country);
    }

    /// <summary>
    /// Purpose: Ensure that null or empty values are not allowed in From method.
    /// Should: Throw ArgumentException.
    /// When: city or country is null or empty.
    /// </summary>
    [Theory]
    [InlineData(null, "Russia")]
    [InlineData("Moscow", null)]
    [InlineData("", "Russia")]
    [InlineData("Moscow", "  ")]
    public void From_Should_Throw_When_Value_Is_Invalid(string? city, string? country)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Location.From(city!, country!));
    }

    /// <summary>
    /// Purpose: Verify that TryFrom correctly creates Location when at least one value is present.
    /// Should: Return Location with trimmed values, using empty string for nulls.
    /// When: One of the values is null or both are present with spaces.
    /// </summary>
    [Theory]
    [InlineData("Moscow", "Russia", "Moscow", "Russia")]
    [InlineData("  Moscow  ", "  Russia  ", "Moscow", "Russia")]
    [InlineData(null, "Russia", "", "Russia")]
    [InlineData("Moscow", null, "Moscow", "")]
    public void TryFrom_Should_Create_Location_When_At_Least_One_Value_Is_Present(string? city, string? country, string expectedCity, string expectedCountry)
    {
        // Act
        var location = Location.TryFrom(city, country);

        // Assert
        Assert.NotNull(location);
        Assert.Equal(expectedCity, location!.City);
        Assert.Equal(expectedCountry, location.Country);
    }

    /// <summary>
    /// Purpose: Verify that TryFrom returns null when both city and country are missing.
    /// Should: Return null.
    /// When: Both values are null, empty or whitespace.
    /// </summary>
    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("  ", "  ")]
    public void TryFrom_Should_Return_Null_When_Both_Values_Are_Empty(string? city, string? country)
    {
        // Act
        var location = Location.TryFrom(city, country);

        // Assert
        Assert.Null(location);
    }
}
