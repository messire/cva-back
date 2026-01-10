using Microsoft.Extensions.DependencyInjection;

namespace CVA.Application.Abstractions;

/// <summary>
/// Executes queries by resolving their corresponding handlers from the service provider.
/// </summary>
public sealed class QueryExecutor(IServiceProvider serviceProvider)
{
    /// <summary>
    /// Executes the specified query asynchronously.
    /// </summary>
    /// <typeparam name="TQuery">The type of query.</typeparam>
    /// <typeparam name="TResponse">The type of response.</typeparam>
    /// <param name="query">The query to execute.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Result{TResponse}"/> representing the result of the operation.</returns>
    public async Task<Result<TResponse>> ExecuteAsync<TQuery, TResponse>(
        TQuery query, 
        CancellationToken ct)
        where TQuery : IQuery<TResponse>
    {
        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
        return await handler.HandleAsync(query, ct);
    }
}