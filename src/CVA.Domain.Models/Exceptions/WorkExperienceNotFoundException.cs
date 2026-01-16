namespace CVA.Domain.Models;

/// <summary>
/// Exception thrown when a work experience entry is not found in the developer profile.
/// </summary>
public sealed class WorkExperienceNotFoundException(WorkExperienceId id)
    : DomainException($"Work experience with id '{id.Value}' was not found.");