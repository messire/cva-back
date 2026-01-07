namespace CVA.Application.Abstractions;

/// <summary>
/// Defines a handler for a query.
/// </summary>
/// <typeparam name="TQuery">The type of query to handle.</typeparam>
/// <typeparam name="TResponse">The type of response returned by the query handler.</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    /// <summary>
    /// Handles the specified query asynchronously.
    /// </summary>
    /// <param name="query">The query to handle.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Result{TResponse}"/> representing the result of the operation.</returns>
    Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken ct);
}