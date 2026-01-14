namespace CVA.Application.ProfileService;

/// <summary>
/// Provides extension methods for mapping requests to domain models or other types.
/// </summary>
public static class RequestsMapping
{
    /// <summary>
    /// Maps a <see cref="LocationDto"/> to a <see cref="Location"/> domain model.
    /// </summary>
    public static Location? ToModel(this LocationDto? dto)
        => dto == null ? null : Location.TryFrom(dto.City, dto.Country);

    /// <summary>
    /// Maps a <see cref="SocialLinksDto"/> to a <see cref="SocialLinks"/> domain model.
    /// </summary>
    public static SocialLinks ToModel(this SocialLinksDto? dto)
        => SocialLinks.Create(dto?.LinkedIn, dto?.GitHub, dto?.Twitter, dto?.Telegram);

    /// <summary>
    /// Maps a <see cref="UpdateProfileContactsRequest"/> to a <see cref="ContactInfo"/> domain model.
    /// </summary>
    public static ContactInfo ToModel(this UpdateProfileContactsRequest request, ContactInfo existing)
        => ContactInfo.Create(
            request.Location?.ToModel(),
            request.Email != null ? EmailAddress.From(request.Email) : existing.Email,
            PhoneNumber.TryFrom(request.Phone),
            Url.TryFrom(request.Website)
        );

    /// <summary>
    /// Maps a <see cref="ReplaceProfileRequest"/> to a <see cref="ContactInfo"/> domain model.
    /// </summary>
    public static ContactInfo ToContactInfo(this ReplaceProfileRequest request)
        => ContactInfo.Create(
            request.Location.ToModel(),
            EmailAddress.From(request.Email ?? string.Empty),
            PhoneNumber.TryFrom(request.Phone),
            Url.TryFrom(request.Website));

    /// <summary>
    /// Maps a <see cref="UpsertWorkExperienceRequest"/> to a <see cref="DateRange"/> domain model.
    /// </summary>
    public static DateRange ToDateRange(this UpsertWorkExperienceRequest request)
        => new(request.StartDate ?? DateOnly.FromDateTime(DateTime.Today), request.EndDate);

    /// <summary>
    /// Maps a <see cref="UpsertWorkExperienceRequest"/> to domain components.
    /// </summary>
    public static (CompanyName Company, Location? Location, RoleTitle Role, WorkDescription Description, DateRange Period, IEnumerable<TechTag> TechStack) ToDomain(
        this UpsertWorkExperienceRequest request)
        => (
            CompanyName.From(request.Company ?? string.Empty),
            request.Location?.ToModel(),
            new RoleTitle(request.Role ?? string.Empty),
            new WorkDescription(request.Description ?? string.Empty),
            request.ToDateRange(),
            request.TechStack.Select(TechTag.From));

    /// <summary>
    /// Maps a <see cref="UpsertProjectRequest"/> to domain components.
    /// </summary>
    public static (ProjectName, ProjectDescription, ProjectLink, ProjectIcon?, IEnumerable<TechTag>) ToDomain(this UpsertProjectRequest request)
        => (ProjectName.From(request.Name ?? string.Empty),
            new ProjectDescription(request.Description ?? string.Empty),
            new ProjectLink(Url.TryFrom(request.LinkUrl)),
            ProjectIcon.TryFrom(request.IconUrl),
            request.TechStack.Select(TechTag.From));

    /// <summary>
    /// Maps a <see cref="WorkExperienceDto"/> to domain components.
    /// </summary>
    public static (CompanyName Company, Location? Location, RoleTitle Role, WorkDescription Description, DateRange Period, IEnumerable<TechTag> TechStack) ToDomain(this WorkExperienceDto dto)
        => (CompanyName.From(dto.Company ?? string.Empty),
            dto.Location?.ToModel(),
            new RoleTitle(dto.Role ?? string.Empty),
            new WorkDescription(dto.Description ?? string.Empty),
            new DateRange(dto.StartDate ?? DateOnly.FromDateTime(DateTime.Today), dto.EndDate),
            dto.TechStack?.Select(TechTag.From) ?? []);

    /// <summary>
    /// Maps a <see cref="ProjectDto"/> to domain components.
    /// </summary>
    public static (ProjectName, ProjectDescription, ProjectLink, ProjectIcon?, IEnumerable<TechTag>) ToDomain(this ProjectDto dto)
        => (ProjectName.From(dto.Name ?? string.Empty),
            new ProjectDescription(dto.Description ?? string.Empty),
            new ProjectLink(Url.TryFrom(dto.LinkUrl)),
            ProjectIcon.TryFrom(dto.IconUrl),
            dto.TechStack?.Select(TechTag.From) ?? []);
}