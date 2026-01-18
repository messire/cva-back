namespace CVA.Application.ProfileService;

/// <summary>
/// Command to update an existing work experience entry in the developer profile.
/// </summary>
/// <param name="WorkExperienceId">The identifier of the work experience entry to update.</param>
/// <param name="Request">The request containing updated work experience details.</param>
public sealed record UpdateWorkExperienceCommand(Guid WorkExperienceId, UpsertWorkExperienceRequest Request) : ICommand<ProfileDto>;