namespace CVA.Domain.Models;

/// <summary>
/// Represents a unique identifier for a developer.
/// </summary>
/// <param name="Value">The unique identifier.</param>
public readonly record struct DeveloperId(Guid Value);