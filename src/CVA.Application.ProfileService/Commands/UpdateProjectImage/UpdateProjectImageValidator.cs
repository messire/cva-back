namespace CVA.Application.ProfileService.UpdateProjectImage;

/// <summary>
/// Validator for the <see cref="UpdateProjectImageCommand"/>.
/// </summary>
public class UpdateProjectImageValidator : AbstractValidator<UpdateProjectImageCommand>
{
    private const long MaxProjectImageSizeBytes = 500 * 1024;

    private static readonly string[] AllowedContentTypes =
    [
        "image/jpeg",
        "image/png",
        "image/webp"
    ];

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProjectImageValidator"/> class.
    /// </summary>
    public UpdateProjectImageValidator()
    {
        RuleFor(command => command.ProjectId)
            .NotEmpty();

        RuleFor(command => command.Content)
            .NotNull();

        RuleFor(command => command.ContentLength)
            .GreaterThan(0)
            .LessThanOrEqualTo(MaxProjectImageSizeBytes)
            .WithMessage($"Project image is too large. Max size is {MaxProjectImageSizeBytes} bytes.");

        RuleFor(command => command.ContentType)
            .NotEmpty()
            .Must(content => AllowedContentTypes.Contains(content, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Unsupported project image type. Allowed: image/jpeg, image/png, image/webp.");

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