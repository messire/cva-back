using CVA.Presentation.Auth;
using Microsoft.Extensions.Caching.Memory;

namespace CVA.Tests.Unit.Presentation.Auth;

/// <summary>
/// Unit tests for <see cref="OAuthStateStore"/>.
/// </summary>
[Trait(Layer.Application, Category.Services)]
public class OAuthStateStoreTests
{
    /// <summary>
    /// Purpose: Verify that created state values can be consumed only once.
    /// Should: Return the stored return URL and remove the state entry.
    /// When: A state value is created and consumed.
    /// </summary>
    [Fact]
    public void Create_And_TryConsume_Should_Work_Once()
    {
        // Arrange
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var store = new OAuthStateStore(cache);
        const string returnUrl = "https://example.com/return";

        // Act
        var state = store.Create(returnUrl, TimeSpan.FromMinutes(5));
        var firstConsume = store.TryConsume(state, out var consumedReturnUrl);
        var secondConsume = store.TryConsume(state, out _);

        // Assert
        Assert.True(firstConsume);
        Assert.Equal(returnUrl, consumedReturnUrl);
        Assert.False(secondConsume);
    }
}
