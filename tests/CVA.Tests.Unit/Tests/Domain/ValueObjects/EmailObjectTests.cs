using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="EmailObject"/> value object.
/// </summary>
[Trait(Layer.Domain, Category.ValueObjects)]
public class EmailObjectTests
{
    /// <summary>
    /// Purpose: Verify that EmailObject correctly normalizes email addresses.
    /// Should: Trim whitespace and convert to lower case.
    /// When: Email string with mixed case and extra spaces is provided.
    /// </summary>
    [Theory]
    [InlineData("test@example.com", "test@example.com")]
    [InlineData("  TEST@example.com  ", "test@example.com")]
    public void Create_Should_Normalize_Email(string input, string expected)
    {
        // Act
        var email = EmailObject.Create(input);

        // Assert
        Assert.Equal(expected, email.Value);
    }

    /// <summary>
    /// Purpose: Ensure that empty or whitespace email strings are rejected.
    /// Should: Throw DomainValidationException with appropriate message.
    /// When: null, empty or whitespace string is provided.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Should_Throw_When_Email_Is_Empty(string? input)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() => EmailObject.Create(input!));
        Assert.Equal("Email must not be empty.", exception.Message);
    }

    /// <summary>
    /// Purpose: Verify validation of email format.
    /// Should: Throw DomainValidationException when '@' is missing or misplaced.
    /// When: Invalid email format string is provided.
    /// </summary>
    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    public void Create_Should_Throw_When_Email_Is_Invalid(string input)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() => EmailObject.Create(input));
        Assert.Equal("Email is invalid.", exception.Message);
    }

    /// <summary>
    /// Purpose: Verify implicit conversion from EmailObject to string.
    /// Should: Return the inner email value as a string.
    /// When: EmailObject is assigned to a string variable.
    /// </summary>
    [Fact]
    public void Implicit_Conversion_To_String_Should_Work()
    {
        // Arrange
        var email = EmailObject.Create("test@example.com");

        // Act
        string result = email;

        // Assert
        Assert.Equal("test@example.com", result);
    }

    /// <summary>
    /// Purpose: Verify ToString method implementation.
    /// Should: Return the inner email value.
    /// When: ToString is called on an EmailObject.
    /// </summary>
    [Fact]
    public void ToString_Should_Return_Value()
    {
        // Arrange
        var email = EmailObject.Create("test@example.com");

        // Act & Assert
        Assert.Equal("test@example.com", email.ToString());
    }
}
