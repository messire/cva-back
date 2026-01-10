using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="VerificationStatus"/> value object.
/// </summary>
[Trait(Layer.Domain, Category.ValueObjects)]
public class VerificationStatusTests
{
    /// <summary>
    /// Purpose: Verify that IsVerified returns true for appropriate verification levels.
    /// Should: Return true for Verified and Premium levels.
    /// When: VerificationLevel is Verified or Premium.
    /// </summary>
    [Theory]
    [InlineData(VerificationLevel.Verified, true)]
    [InlineData(VerificationLevel.Premium, true)]
    [InlineData(VerificationLevel.NotVerified, false)]
    public void IsVerified_Should_Return_Correct_Value(VerificationLevel level, bool expected)
    {
        // Act
        var status = new VerificationStatus(level);

        // Assert
        Assert.Equal(expected, status.IsVerified);
    }

    /// <summary>
    /// Purpose: Verify Default property returns NotVerified status.
    /// Should: Return VerificationStatus with NotVerified level.
    /// When: Default is called.
    /// </summary>
    [Fact]
    public void Default_Should_Return_NotVerified()
    {
        // Act
        var status = VerificationStatus.Default;

        // Assert
        Assert.Equal(VerificationLevel.NotVerified, status.Value);
        Assert.False(status.IsVerified);
    }
}
