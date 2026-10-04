using FluentValidation;

namespace Languages.WebApi.Models.Requests.Language;

public sealed class SetLanguageBackgroundImageRequest
{
    public IFormFile BackgroundImage { get; set; }
}

public class SetLanguageBackgroundImageRequestValidator : AbstractValidator<SetLanguageBackgroundImageRequest>
{
    public SetLanguageBackgroundImageRequestValidator()
    {
        RuleFor(l => l.BackgroundImage)
            .NotNull()
            .WithMessage("Background image is required.");
    }
}
