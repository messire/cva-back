namespace CVA.Domain.Models;

/// <summary>
/// Represents a unique identifier for a work experience.
/// </summary>
/// <param name="Value">The unique identifier.</param>
public readonly record struct WorkExperienceId(Guid Value);