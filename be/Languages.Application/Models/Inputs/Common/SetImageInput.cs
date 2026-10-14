using FluentValidation;
using SixLabors.ImageSharp;

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
    private const int MinWidth = 500;
    private const int MinHeight = 500;
    private static readonly string[] AcceptedImageFormats = ["image/jpeg", "image/png"];

    public SetImageInputValidator()
    {
        RuleFor(i => i.ImageStream)
            .Cascade(CascadeMode.Stop)
            .NotNull()
                .WithMessage("Background image is required.")
            .Must(stream => stream.CanRead)
                .WithMessage("Background image could not be read.")
            .Must(stream => stream.CanSeek)
                .WithMessage("Background image could not be read.")
            .MustAsync(async (stream, cancellationToken) => 
                await ValidateDimensions(stream, cancellationToken))
            .WithMessage("Background image must be at least 500px in width and 500px in height.");

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

    private static async Task<bool> ValidateDimensions(Stream stream, CancellationToken cancellationToken)
    {
        var startingPosition = stream.Position;

        try
        {
            stream.Position = 0;

            var imageInfo = await Image.IdentifyAsync(stream, cancellationToken);

            return imageInfo.Width >= MinWidth && imageInfo.Height >= MinHeight;
        }
        catch (Exception exception) when (exception is UnknownImageFormatException or InvalidImageContentException)
        {
            return false;
        }
        finally
        {
            stream.Position = startingPosition;
        }
    }
}
