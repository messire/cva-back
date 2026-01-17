using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CVA.Infrastructure.Storage.S3;

/// <summary>
/// Dependency injection extensions for S3-compatible storage.
/// </summary>
public static class DiConfig
{
    /// <summary>
    /// Adds S3-compatible object storage (Railway Bucket / MinIO).
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    public static IServiceCollection AddCvaS3Storage(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<S3Options>()
            .Bind(configuration.GetSection(S3Options.Path))
            .Validate(o =>
                    !string.IsNullOrWhiteSpace(o.Endpoint) &&
                    !string.IsNullOrWhiteSpace(o.Bucket) &&
                    !string.IsNullOrWhiteSpace(o.AccessKeyId) &&
                    !string.IsNullOrWhiteSpace(o.SecretAccessKey),
                "S3 options are invalid. Ensure S3:Endpoint, S3:Bucket, S3:AccessKeyId, S3:SecretAccessKey are set.")
            .ValidateOnStart();

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var o = sp.GetRequiredService<IOptions<S3Options>>().Value;

            var cfg = new AmazonS3Config
            {
                ServiceURL = o.Endpoint,
                ForcePathStyle = o.ForcePathStyle
            };

            var creds = new BasicAWSCredentials(o.AccessKeyId, o.SecretAccessKey);
            return new AmazonS3Client(creds, cfg);
        });

        services.AddSingleton<S3ObjectStorage>();

        return services;
    }
}