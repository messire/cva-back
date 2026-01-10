namespace CVA.Application.Abstractions;

/// <summary>
/// Defines a handler for a command.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle.</typeparam>
/// <typeparam name="TResponse">The type of response returned by the command handler.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    /// <summary>
    /// Handles the specified command asynchronously.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Result{TResponse}"/> representing the result of the operation.</returns>
    Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken ct);
}