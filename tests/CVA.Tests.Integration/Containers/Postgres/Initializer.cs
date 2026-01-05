using static CVA.Tests.Integration.Postgres.Constants;

namespace CVA.Tests.Integration.Postgres;

/// <summary>
/// Postgres initializer.
/// </summary>
internal static class Initializer
{
    /// <summary>
    /// Initialize <see cref="PostgreSqlContainer"/>.
    /// </summary>
    /// <param name="sharedNetwork"><see cref="INetwork"/> insatnce.</param>
    /// <returns>Configured <see cref="PostgreSqlContainer"/> instance.</returns>
    public static IContainer Init(INetwork sharedNetwork)
    {
        var containerName = ContainerBaseName + Guid.CreateVersion7();
        return new PostgreSqlBuilder()
            .WithImage(Images.PostgresImage)
            .WithUsername(UserName)
            .WithPassword(Password)
            .WithDatabase(Database)
            .WithNetwork(sharedNetwork)
            .WithName(containerName)
            .WithNetworkAliases(Alias)
            .Build();
    }
}
