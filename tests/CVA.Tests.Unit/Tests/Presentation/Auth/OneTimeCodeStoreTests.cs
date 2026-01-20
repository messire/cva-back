using CVA.Presentation.Auth;
using Microsoft.Extensions.Caching.Memory;

namespace CVA.Tests.Unit.Presentation.Auth;

/// <summary>
/// Unit tests for <see cref="OneTimeCodeStore"/>.
/// </summary>
[Trait(Layer.Application, Category.Services)]
public class OneTimeCodeStoreTests
{
    /// <summary>
    /// Purpose: Verify that one-time codes can be consumed only once.
    /// Should: Return the stored token and remove the code entry.
    /// When: A one-time code is created and consumed.
    /// </summary>
    [Fact]
    public void Create_And_TryConsume_Should_Work_Once()
    {
        // Arrange
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var store = new OneTimeCodeStore(cache);
        var token = new AuthTokenDto("access", "refresh", Guid.NewGuid());

        // Act
        var code = store.Create(token, TimeSpan.FromMinutes(5));
        var firstConsume = store.TryConsume(code, out var consumedToken);
        var secondConsume = store.TryConsume(code, out _);

        // Assert
        Assert.True(firstConsume);
        Assert.Equal(token, consumedToken);
        Assert.False(secondConsume);
    }
}
