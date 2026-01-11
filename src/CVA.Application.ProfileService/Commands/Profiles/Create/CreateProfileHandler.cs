namespace CVA.Application.ProfileService;

/// <summary>
/// Handles creation of the current authenticated developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class CreateProfileHandler(
    IDeveloperProfileRepository repository,
    ICurrentUserAccessor userAccessor)
    : ICommandHandler<CreateProfileCommand, DeveloperProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<DeveloperProfileDto>> HandleAsync(CreateProfileCommand command, CancellationToken ct)
    {
        if (!userAccessor.IsAuthenticated)
        {
            return AppError.Failure("User is not authenticated.");
        }

        var existing = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (existing is not null)
        {
            return AppError.Conflict("Profile already exists.");
        }

        var request = command.Request;
        var now = DateTimeOffset.UtcNow;

        var contactInfo = ContactInfo.Create(
            request.Location.ToModel(),
            EmailAddress.From(request.Email),
            PhoneNumber.TryFrom(request.Phone),
            Url.TryFrom(request.Website));

        var profile = DeveloperProfile.Create(
            new DeveloperId(userAccessor.UserId),
            PersonName.From(request.FirstName, request.LastName),
            request.Role != null ? new RoleTitle(request.Role) : null,
            ProfileSummary.TryFrom(request.Summary),
            Avatar.TryFrom(request.AvatarUrl),
            contactInfo,
            request.SocialLinks.ToModel(),
            verification: VerificationStatus.Default,
            openToWork: new OpenToWorkStatus(request.OpenToWork),
            yearsOfExperience: YearsOfExperience.From(request.YearsOfExperience),
            now);

        var created = await repository.CreateAsync(profile, ct);
        return created.ToDto();
    }
}