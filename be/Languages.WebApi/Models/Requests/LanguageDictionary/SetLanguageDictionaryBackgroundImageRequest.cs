using FluentValidation;
using Languages.WebApi.Models.Common;

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
            .SetValidator(new ImageFileValidator());
    }
}
