namespace CVA.Application.Abstractions;

/// <summary>
/// Marker interface to represent a query that returns a <see cref="Result{T}"/>.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the query.</typeparam>
public interface IQuery<TResponse>;