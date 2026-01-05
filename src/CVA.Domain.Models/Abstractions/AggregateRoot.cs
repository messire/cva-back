namespace CVA.Domain.Models;

/// <summary>
/// Base class for aggregate roots.
/// </summary>
public abstract class AggregateRoot
{
    /// <summary>
    /// Replaces the contents of the target list with elements from the source collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="target">The list to be cleared and populated with the new elements.</param>
    /// <param name="source">The source collection from which elements are retrieved. Null values in the collection are not allowed.</param>
    /// <param name="normalize">An optional function to normalize or transform each element before adding it to the target list.</param>
    /// <exception cref="DomainValidationException">Thrown if the source collection contains null elements.</exception>
    protected static void ReplaceList<T>(
        List<T> target,
        IEnumerable<T>? source,
        Func<T, T>? normalize = null)
    {
        target.Clear();

        if (source is null) return;
        foreach (var item in source)
        {
            if (item is null)
            {
                throw new DomainValidationException("Collection must not contain null elements.");
            }

            target.Add(normalize is null ? item : normalize(item));
        }
    }
}