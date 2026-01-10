namespace CVA.Application.ProfileService;

/// <summary>
/// Command to create a new project in the developer profile.
/// </summary> 
/// <param name="Request">The request containing project details.</param>
public sealed record CreateProjectCommand(UpsertProjectRequest Request) : ICommand<DeveloperProfileDto>;