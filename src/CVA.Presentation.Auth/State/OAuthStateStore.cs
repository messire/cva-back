using Microsoft.Extensions.Caching.Memory;

namespace CVA.Presentation.Auth;

/// <summary>
/// In-memory OAuth state store.
/// </summary>
/// <param name="cache">Memory cache.</param>
internal sealed class OAuthStateStore(IMemoryCache cache) : IOAuthStateStore
{
    /// <inheritdoc />
    public string Create(string returnUrl, TimeSpan ttl)
    {
        var state = Guid.NewGuid().ToString("N");
        cache.Set(Key(state), returnUrl, ttl);
        return state;
    }

    /// <inheritdoc />
    public bool TryConsume(string state, out string returnUrl)
    {
        if (!cache.TryGetValue(Key(state), out returnUrl!)) return false;

        cache.Remove(Key(state));
        return true;
    }

    private static string Key(string state) => $"oauth:state:{state}";
}