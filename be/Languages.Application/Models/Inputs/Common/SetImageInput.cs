using FluentValidation;

namespace Languages.Application.Models.Inputs.Common;

public sealed class SetImageInput
{
    public required Stream ImageStream { get; init; }
    public required string ContentType { get; init; }
    public required long Length { get; init; }
}

public class SetImageInputValidator : AbstractValidator<SetImageInput>
{
    private const long MaxImageSizeBytes = 5 * 1024 * 1024;
    private static readonly string[] AcceptedImageFormats = ["image/jpeg", "image/png"];

    public SetImageInputValidator()
    {
        RuleFor(i => i.ImageStream)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Background image is required.")
            .Must(stream => stream.CanRead)
            .WithMessage("Background image be readable.");

        RuleFor(i => i.Length)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("Background image must not be empty.")
            .LessThanOrEqualTo(MaxImageSizeBytes)
            .WithMessage("Background image must not exceed 5 MiB.");

        RuleFor(i => i.ContentType)
            .Must(type => AcceptedImageFormats.Contains(type, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Only JPEG and PNG images are supported.");
    }
}
