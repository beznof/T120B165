using FluentValidation;

namespace Languages.WebApi.Models.Requests.LanguageDictionary;

public class BaseLanguageDictionaryRequest
{
    public required int LanguageId { get; set; }
}

public class BaseLanguageDictionaryRequestValidator : AbstractValidator<BaseLanguageDictionaryRequest>
{
    public BaseLanguageDictionaryRequestValidator()
    {
        RuleFor(r => r.LanguageId)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
    }
}