using FluentValidation;

namespace Languages.WebApi.Models.Requests.Dictionary;

public sealed class SetDictionaryBackgroundImageRequest
{
    public IFormFile BackgroundImage { get; set; }
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
