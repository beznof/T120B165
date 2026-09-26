using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Language.Requests.ReadMany;

public class ReadManyLanguagesRequestValidator : AbstractValidator<ReadManyLanguagesRequest>
{
    public ReadManyLanguagesRequestValidator()
    {
        Include(new ReadManyRequestValidator());
        
        RuleForEach(l => l.LanguageFamilyIDs)
            .GreaterThan(0)
            .WithMessage("LanguageFamily ID must be a positive integer.");
    }
}
