namespace CVA.Tests.Unit.Application.Contracts;

/// <summary>
/// Unit tests for the <see cref="Result{T}"/> class.
/// </summary>
[Trait(Layer.Application, Category.Contracts)]
public class ResultTests
{
    /// <summary>
    /// Purpose: Verify that Ok method creates a success result.
    /// Should: Set IsSuccess to true, contain the value, and have no error.
    /// When: A valid value is provided to the factory method.
    /// </summary>
    [Theory, CvaAutoData]
    public void Ok_Should_Return_Success_Result(string value)
    {
        // Act
        var result = Result<string>.Ok(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    /// <summary>
    /// Purpose: Verify that Fail method creates a failure result.
    /// Should: Set IsSuccess to false, contain the error message, and have default value.
    /// When: An error string is provided to the factory method.
    /// </summary>
    [Theory, CvaAutoData]
    public void Fail_Should_Return_Failure_Result(string errorMessage)
    {
        // Act
        var result = Result<int>.Fail(errorMessage);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(errorMessage, result.Error);
        Assert.Equal(0, result.Value);
    }

    /// <summary>
    /// Purpose: Ensure that Result can handle null values if type T is nullable.
    /// Should: Correctly encapsulate null as a valid success value.
    /// When: Ok is called with null.
    /// </summary>
    [Fact]
    public void Ok_Should_Handle_Null_Value()
    {
        // Act
        var result = Result<string?>.Ok(null);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
    }
}