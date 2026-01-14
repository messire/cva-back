namespace CVA.Application.ProfileService.UpdateProfileAvatar;

/// <summary>
/// Validator for the <see cref="UpdateProfileAvatarCommand"/>.
/// </summary>
public sealed class UpdateProfileAvatarValidator : AbstractValidator<UpdateProfileAvatarCommand>
{
    private const long MaxAvatarSizeBytes = 500 * 1024;

    private static readonly string[] AllowedContentTypes =
    [
        "image/jpeg",
        "image/png",
        "image/webp"
    ];

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProfileAvatarValidator"/> class.
    /// </summary>
    public UpdateProfileAvatarValidator()
    {
        RuleFor(command => command.Content)
            .NotNull();

        RuleFor(command => command.ContentLength)
            .GreaterThan(0)
            .LessThanOrEqualTo(MaxAvatarSizeBytes)
            .WithMessage($"Avatar image is too large. Max size is {MaxAvatarSizeBytes} bytes.");

        RuleFor(command => command.ContentType)
            .NotEmpty()
            .Must(ct => AllowedContentTypes.Contains(ct, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Unsupported avatar image type. Allowed: image/jpeg, image/png, image/webp.");

        RuleFor(command => command.PublicBaseUrl)
            .NotEmpty()
            .Must(value => Uri.IsWellFormedUriString(value, UriKind.Absolute))
            .WithMessage("PublicBaseUrl must be an absolute URL.");

        RuleFor(command => command.MediaRequestPath)
            .NotEmpty()
            .Must(path => path.StartsWith('/'))
            .WithMessage("MediaRequestPath must start with '/'.");
    }
}