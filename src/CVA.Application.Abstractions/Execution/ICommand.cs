namespace CVA.Application.Abstractions;

/// <summary>
/// Marker interface to represent a command that returns a <see cref="Result{T}"/>.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the command.</typeparam>
public interface ICommand<TResponse>;