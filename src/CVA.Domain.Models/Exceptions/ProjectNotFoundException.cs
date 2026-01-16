namespace CVA.Domain.Models;

/// <summary>
/// Exception thrown when a project is not found in the developer profile.
/// </summary>
public sealed class ProjectNotFoundException(ProjectId id)
    : DomainException($"Project with id '{id.Value}' was not found.");