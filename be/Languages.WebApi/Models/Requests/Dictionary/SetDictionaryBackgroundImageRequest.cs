using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Languages.WebApi.Models.Requests.Dictionary;

public sealed class SetDictionaryBackgroundImageRequest
{
    /// <summary>
    /// The background image.
    /// Required.
    /// Must not be empty.
    /// Must not exceed 5 MiB in size.
    /// Must be at least 500px wide and 500px tall.
    /// Must be in JPEG or PNG format.
    /// </summary>
    [Required(ErrorMessage = "Background image is required.")]
    public IFormFile? BackgroundImage { get; set; }
}

public class SetDictionaryBackgroundImageRequestValidator : AbstractValidator<SetDictionaryBackgroundImageRequest>
{
    public SetDictionaryBackgroundImageRequestValidator()
    {
        RuleFor(d => d.BackgroundImage)
            .NotNull()
            .WithMessage("Background image is required.");
    }
}
