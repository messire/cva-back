using CVA.Application.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace CVA.Presentation.Auth;

/// <summary>
/// In-memory one-time exchange code store.
/// </summary>
/// <param name="cache">Memory cache.</param>
internal sealed class OneTimeCodeStore(IMemoryCache cache) : IOneTimeCodeStore
{
    /// <inheritdoc />
    public string Create(AuthTokenDto token, TimeSpan ttl)
    {
        var code = Guid.NewGuid().ToString("N");
        cache.Set(Key(code), token, ttl);
        return code;
    }

    /// <inheritdoc />
    public bool TryConsume(string code, out AuthTokenDto token)
    {
        if (!cache.TryGetValue(Key(code), out token!)) return false;
        cache.Remove(Key(code));
        return true;
    }

    private static string Key(string code) => $"oauth:exchange:{code}";
}