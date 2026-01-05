namespace CVA.Tests.Integration.Postgres;

/// <summary>
/// Postgres container constants.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// Database alias.
    /// </summary>
    public const string Alias = "postgres-db";

    /// <summary>
    /// Base Postgres container name.
    /// </summary>
    public const string ContainerBaseName = "postgres-test";

    /// <summary>
    /// Database name.
    /// </summary>
    public const string Database = "postgres";

    /// <summary>
    /// Database user.
    /// </summary>
    public const string UserName = "postgres";

    /// <summary>
    /// Database pass.
    /// </summary>
    public const string Password = "postgres";

    /// <summary>
    /// Database port.
    /// </summary>
    public const int Port = 5432;
}
