using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.Exceptions;

/// <summary>
/// Unit tests for domain exceptions.
/// </summary>
[Trait(Layer.Domain, Category.Exceptions)]
public class DomainExceptionTests
{
    /// <summary>
    /// Purpose: Verify that DomainValidationException correctly stores message and inner exception.
    /// Should: Set properties through base constructor.
    /// When: Created via both standard constructors.
    /// </summary>
    [Fact]
    public void DomainValidationException_Should_Initialize_Properties()
    {
        // Arrange
        var message = "Error";
        var inner = new Exception("Inner");

        // Act
        var ex1 = new DomainValidationException(message);
        var ex2 = new DomainValidationException(message, inner);

        // Assert
        Assert.Equal(message, ex1.Message);
        Assert.Equal(message, ex2.Message);
        Assert.Equal(inner, ex2.InnerException);
    }
}
