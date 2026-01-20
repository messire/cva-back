namespace CVA.Application.ProfileService;

/// <summary>
/// Command to update an existing project in the developer profile.
/// </summary>
/// <param name="ProjectId">The identifier of the project to update.</param>
/// <param name="Request">The request containing updated project details.</param>
public sealed record UpdateProjectCommand(Guid ProjectId, UpsertProjectRequest Request) : ICommand<ProfileDto>;