using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageDictionary.Requests;

public class SetLanguageDictionaryBackgroundImageRequestValidator : AbstractValidator<SetLanguageDictionaryBackgroundImageRequest>
{
    public SetLanguageDictionaryBackgroundImageRequestValidator()
    {
            RuleFor(d => d.BackgroundImage)
                .SetValidator(new ImageFileValidator());
    }
}
