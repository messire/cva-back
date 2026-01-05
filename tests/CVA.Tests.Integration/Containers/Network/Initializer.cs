using static CVA.Tests.Integration.Network.Constants;

namespace CVA.Tests.Integration.Network;

/// <summary>
/// Network initializer.
/// </summary>
internal static class Initializer
{
    /// <summary>
    /// Initialize <see cref="INetwork"/>.
    /// </summary>
    /// <returns><see cref="INetwork"/> insatnce.</returns>
    public static INetwork Init()
    {
        var containerName = ContainerBaseName + Guid.CreateVersion7();
        return new NetworkBuilder().WithName(containerName).Build();
    }
}
