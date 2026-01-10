namespace CVA.Application.ProfileService;

/// <summary>
/// Command to delete a project from the developer profile.
/// </summary>
/// <param name="ProjectId">The identifier of the project to delete.</param>
public sealed record DeleteProjectCommand(Guid ProjectId) : ICommand<DeveloperProfileDto>;