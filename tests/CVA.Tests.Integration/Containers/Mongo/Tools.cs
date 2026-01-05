using CVA.Infrastructure.Mongo;

namespace CVA.Tests.Integration.Mongo;

internal static class Tools
{
    public static MongoOptions GetConfiguration(string connectionString)
    {
        var options = GetOptions(connectionString);
        return options;
    }

    private static MongoOptions GetOptions(string connectionString)
        => new()
        {
            Connection = connectionString,
            DatabaseName = Constants.Database,
        };
}