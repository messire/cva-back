using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Provides configuration for dependency injection related to the MongoDB database.
/// </summary>
public static class DiConfig
{
    /// <summary>
    /// Registers MongoDB services and configurations into the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection used for dependency injection.</param>
    /// <param name="configuration">The application configuration instance.</param>
    public static void RegisterMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoOptions = configuration.GetRequiredSection(MongoOptions.Path).Get<MongoOptions>();
        ArgumentNullException.ThrowIfNull(mongoOptions);

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
        {
            BsonClassMap.RegisterClassMap<User>(classMap =>
            {
                classMap.AutoMap();
                classMap.MapIdMember(user => user.Id);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Work)))
        {
            BsonClassMap.RegisterClassMap<Work>(classMap =>
            {
                classMap.AutoMap();
            });
        }

        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoOptions.Connection));
        services.AddSingleton(mongoOptions);
        services.AddScoped<IUserRepository, UserMongoRepository>();
    }
}