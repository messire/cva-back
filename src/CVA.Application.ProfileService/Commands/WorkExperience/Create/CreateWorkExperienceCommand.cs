namespace CVA.Application.ProfileService;

/// <summary>
/// Command to add a new work experience entry to the developer profile.
/// </summary>
/// <param name="Request">The request containing work experience details.</param>
public sealed record CreateWorkExperienceCommand(UpsertWorkExperienceRequest Request) : ICommand<DeveloperProfileDto>;