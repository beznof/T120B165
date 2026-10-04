using FluentValidation;

namespace Languages.WebApi.Models.Requests.LanguageDictionary;

public sealed class SetLanguageDictionaryBackgroundImageRequest
{
    public IFormFile BackgroundImage { get; set; }
}

public class SetLanguageDictionaryBackgroundImageRequestValidator : AbstractValidator<SetLanguageDictionaryBackgroundImageRequest>
{
    public SetLanguageDictionaryBackgroundImageRequestValidator()
    {
        RuleFor(d => d.BackgroundImage)
            .NotNull()
            .WithMessage("Background image is required.");
    }
}
