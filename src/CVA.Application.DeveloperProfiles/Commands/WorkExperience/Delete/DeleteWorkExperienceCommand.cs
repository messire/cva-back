namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Command to delete a work experience entry from the developer profile.
/// </summary>
/// <param name="WorkExperienceId">The identifier of the work experience entry to delete.</param>
public sealed record DeleteWorkExperienceCommand(Guid WorkExperienceId) : ICommand<DeveloperProfileDto>;