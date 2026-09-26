using FluentValidation;

namespace Languages.WebApi.Models.Common;

public class ImageFileValidator : AbstractValidator<IFormFile>
{
    private const long MaxImageSizeBytes = 5 * 1024 * 1024;
    private static readonly string[] AcceptedImageFormats = ["image/jpeg", "image/png"];

    public ImageFileValidator()
    {
        RuleFor(f => f.Length)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("Background image must not be empty.")
            .LessThanOrEqualTo(MaxImageSizeBytes)
            .WithMessage("Background image must not exceed 5 MiB.");

        RuleFor(f => f.ContentType)
            .Must(type => AcceptedImageFormats.Contains(type, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Only JPEG and PNG images are supported.");
    }
}
