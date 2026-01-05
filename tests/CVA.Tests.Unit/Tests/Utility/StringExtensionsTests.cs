using CVA.Tools.Common;

namespace CVA.Tests.Unit.Utility;

/// <summary>
/// Unit tests for the <see cref="StringExtensions"/> class.
/// </summary>
[Trait(Layer.Utility, Category.Helpers)]
public class StringExtensionsTests
{
    /// <summary>
    /// Purpose: Verify standard conversion from PascalCase/camelCase to snake_case.
    /// Should: Lowercase characters and add underscores before uppercase letters.
    /// When: Input is a typical C# property name.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("UserId", "user_id")]
    [InlineCvaAutoData("CompanyName", "company_name")]
    [InlineCvaAutoData("WorkExperience", "work_experience")]
    [InlineCvaAutoData("simple", "simple")]
    public void ToSnakeCase_Should_Convert_Property_Names(string input, string expected)
    {
        // Act
        var result = input.ToSnakeCase();

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Purpose: Verify handling of abbreviations in property names.
    /// Should: Add underscores between each capital letter (standard behavior for this algorithm).
    /// When: Input contains uppercase sequences like DTO or URL.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("UserDTO", "user_d_t_o")]
    [InlineCvaAutoData("URL", "u_r_l")]
    [InlineCvaAutoData("IPAddress", "i_p_address")]
    public void ToSnakeCase_Should_Handle_Abbreviations_By_Algorithm_Rules(string input, string expected)
    {
        // Act
        var result = input.ToSnakeCase();

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Purpose: Verify handling of null, empty or white-space strings.
    /// Should: Return empty string without throwing exceptions.
    /// When: Input is null, empty or whitespace.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("")]
    [InlineCvaAutoData(null)]
    [InlineCvaAutoData("   ")]
    public void ToSnakeCase_Should_Return_Empty_On_Invalid_Input(string? input)
    {
        // Act
        var result = input!.ToSnakeCase();

        // Assert
        Assert.Empty(result);
    }

    /// <summary>
    /// Purpose: Verify handling of single character strings.
    /// Should: Return lowercased character without underscore.
    /// When: Input length is 1.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("A", "a")]
    [InlineCvaAutoData("z", "z")]
    public void ToSnakeCase_Should_Handle_Single_Character(string input, string expected)
    {
        // Act
        var result = input.ToSnakeCase();

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Purpose: Verify that numbers are preserved without extra underscores.
    /// Should: Maintain digits as they are.
    /// When: Property name contains numbers.
    /// </summary>
    [Theory]
    [InlineCvaAutoData("Version123", "version123")]
    [InlineCvaAutoData("AddressLine1", "address_line1")]
    public void ToSnakeCase_Should_Preserve_Digits(string input, string expected)
    {
        // Act
        var result = input.ToSnakeCase();

        // Assert
        Assert.Equal(expected, result);
    }
}