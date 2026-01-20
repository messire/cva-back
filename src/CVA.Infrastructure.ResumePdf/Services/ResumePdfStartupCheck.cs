using Amazon.S3;
using CVA.Infrastructure.Storage.S3;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CVA.Infrastructure.ResumePdf;

/// <summary>
/// Validates that required dependencies for resume PDF generation are registered.
/// </summary>
/// <param name="sp">Service provider.</param>
internal sealed class ResumePdfStartupCheck(IServiceProvider sp) : IHostedService
{
    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = sp.CreateScope();

        scope.ServiceProvider.GetRequiredService<IAmazonS3>();
        scope.ServiceProvider.GetRequiredService<S3ObjectStorage>();

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}