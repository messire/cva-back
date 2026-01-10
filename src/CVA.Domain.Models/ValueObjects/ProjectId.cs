namespace CVA.Domain.Models;

/// <summary>
/// Represents a unique identifier for a project.
/// </summary>
/// <param name="Value">The unique identifier.</param>
public readonly record struct ProjectId(Guid Value);