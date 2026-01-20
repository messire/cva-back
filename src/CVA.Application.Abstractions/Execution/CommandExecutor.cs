using Microsoft.Extensions.DependencyInjection;

namespace CVA.Application.Abstractions;

/// <summary>
/// Executes commands by resolving their corresponding handlers from the service provider.
/// </summary>
public sealed class CommandExecutor(IServiceProvider serviceProvider)
{
    /// <summary>
    /// Executes the specified command asynchronously.
    /// </summary>
    /// <typeparam name="TCommand">The type of command.</typeparam>
    /// <typeparam name="TResponse">The type of response.</typeparam>
    /// <param name="command">The command to execute.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Result{TResponse}"/> representing the result of the operation.</returns>
    public async Task<Result<TResponse>> ExecuteAsync<TCommand, TResponse>(
        TCommand command, 
        CancellationToken ct)
        where TCommand : ICommand<TResponse>
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
        return await handler.HandleAsync(command, ct);
    }
}