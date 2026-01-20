namespace CVA.Application.ProfileService;

/// <summary>
/// Query to get a developer profile by its identifier.
/// </summary>
/// <param name="Id">The identifier of the developer profile.</param>
public sealed record GetProfileByIdQuery(Guid Id) : IQuery<ProfileDto>;