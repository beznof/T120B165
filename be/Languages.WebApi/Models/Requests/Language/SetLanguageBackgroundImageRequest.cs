using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Requests.Language;

public sealed class SetLanguageBackgroundImageRequest
{
    public IFormFile BackgroundImage { get; set; }
}

public class SetLanguageBackgroundImageRequestValidator : AbstractValidator<SetLanguageBackgroundImageRequest>
{
    public SetLanguageBackgroundImageRequestValidator()
    {
        RuleFor(l => l.BackgroundImage!)
            .SetValidator(new ImageFileValidator());
    }
}