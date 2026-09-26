using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Language.Requests.UpdateBackgroundImage;

public class UpdateLanguageBackgroundImageRequestValidator : AbstractValidator<UpdateLanguageBackgroundImageRequest>
{
    public UpdateLanguageBackgroundImageRequestValidator()
    {
        RuleFor(l => l.BackgroundImage)
            .NotNull()
            .When(l => !l.RemoveBackgroundImage)
            .WithMessage("Supply a background image or request its removal.");

        When(l => l.BackgroundImage != null, () =>
        {
            RuleFor(l => l.BackgroundImage!)
                .SetValidator(new ImageFileValidator());
        });

        RuleFor(l => l.RemoveBackgroundImage)
            .Must((l, remove) => !remove || l.BackgroundImage is null)
            .WithMessage("An image can't be removed and updated at the same time.");
    }
}
