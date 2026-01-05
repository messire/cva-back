using Microsoft.Extensions.Hosting;

namespace CVA.Infrastructure.Postgres;

/// <summary>
/// A hosted service responsible for applying any pending Entity Framework Core migrations at application startup.
/// </summary>
internal sealed class DbMigrationHostedService(IServiceProvider sp) : IHostedService
{
    /// <inheritdoc />
    public async Task StartAsync(CancellationToken ct)
    {
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PostgresContext>();
        await db.Database.MigrateAsync(ct);
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken ct)
        => Task.CompletedTask;
}
