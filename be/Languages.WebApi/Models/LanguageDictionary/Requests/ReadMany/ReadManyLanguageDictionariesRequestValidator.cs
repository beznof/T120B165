using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageDictionary.Requests.ReadMany;

public class ReadManyLanguageDictionariesRequestValidator : AbstractValidator<ReadManyLanguageDictionariesRequest>
{
    public ReadManyLanguageDictionariesRequestValidator()
    {
        Include(new ReadManyRequestValidator());
        
        RuleFor(d => d.LanguageID)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
        
    }
}
