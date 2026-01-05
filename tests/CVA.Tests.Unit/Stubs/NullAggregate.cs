using CVA.Domain.Models;

namespace CVA.Tests.Unit.Stubs;

/// <summary>
/// Stub aggregate for testing purposes.
/// </summary>
internal class NullAggregate : AggregateRoot
{
    public new static void ReplaceList<T>(List<T> target, IEnumerable<T>? source, Func<T, T>? normalize = null)
        => AggregateRoot.ReplaceList(target, source, normalize);
}