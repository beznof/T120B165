using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageDictionary.Requests.UpdateBackgroundImage;

public class UpdateLanguageDictionaryBackgroundImageRequestValidator : AbstractValidator<UpdateLanguageDictionaryBackgroundImageRequest>
{
    public UpdateLanguageDictionaryBackgroundImageRequestValidator()
    {
        RuleFor(d => d.BackgroundImage)
            .NotNull()
            .When(d => !d.RemoveBackgroundImage)
            .WithMessage("Supply a background image or request its removal.");

        When(d => d.BackgroundImage != null, () =>
        {
            RuleFor(d => d.BackgroundImage!)
                .SetValidator(new ImageFileValidator());
        });

        RuleFor(d => d.RemoveBackgroundImage)
            .Must((d, remove) => !remove || d.BackgroundImage is null)
            .WithMessage("An image can't be removed and updated at the same time.");
    }
}
