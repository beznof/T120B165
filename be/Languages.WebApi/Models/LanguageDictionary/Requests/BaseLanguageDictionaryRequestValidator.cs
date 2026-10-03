using FluentValidation;

namespace Languages.WebApi.Models.LanguageDictionary.Requests;

public class BaseLanguageDictionaryRequestValidator : AbstractValidator<BaseLanguageDictionaryRequest>
{
    public BaseLanguageDictionaryRequestValidator()
    {
        RuleFor(r => r.LanguageId)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
    }
}