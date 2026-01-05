using static CVA.Tests.Integration.Mongo.Constants;

namespace CVA.Tests.Integration.Mongo;

/// <summary>
/// MongoDb initializer.
/// </summary>
internal static class Initializer
{
    /// <summary>
    /// Initialize <see cref="MongoDbContainer"/>.
    /// </summary>
    /// <param name="sharedNetwork"><see cref="INetwork"/> insatnce.</param>
    /// <returns>Configured <see cref="MongoDbContainer"/> instance.</returns>
    public static IContainer Init(INetwork sharedNetwork)
    {
        var containerName = ContainerBaseName + Guid.CreateVersion7();
        return new MongoDbBuilder()
            .WithImage(Images.MongoImage)
            .WithUsername(UserName)
            .WithPassword(Password)
            .WithNetwork(sharedNetwork)
            .WithName(containerName)
            .WithNetworkAliases(Alias)
            .Build();
    }
}
