namespace CVA.Domain.Models;

/// <summary>
/// Base class for aggregate roots.
/// </summary>
public abstract class AggregateRoot
{
    /// <summary>
    /// The date and time when the developer profile was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.Now;

    /// <summary>
    /// The date and time when the developer profile was last updated.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; private set; }

    /// <summary>
    /// Set last time update property.
    /// </summary>
    /// <param name="now"></param>
    protected void Touch(DateTimeOffset now)
        => UpdatedAt = now;
}