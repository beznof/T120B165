using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Language.Requests;

public class SetLanguageBackgroundImageRequestValidator : AbstractValidator<SetLanguageBackgroundImageRequest>
{
    public SetLanguageBackgroundImageRequestValidator()
    {
        RuleFor(l => l.BackgroundImage!)
            .SetValidator(new ImageFileValidator());
    }
}
