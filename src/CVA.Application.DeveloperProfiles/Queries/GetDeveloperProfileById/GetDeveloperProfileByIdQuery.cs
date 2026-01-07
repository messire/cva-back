namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Query to get a developer profile by its identifier.
/// </summary>
/// <param name="Id">The identifier of the developer profile.</param>
public sealed record GetDeveloperProfileByIdQuery(Guid Id) : IQuery<DeveloperProfileDto>;