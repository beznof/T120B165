using FluentValidation;

namespace Languages.WebApi.Models.LanguageDictionary.Requests.Read;

public class ReadLanguageDictionaryRequestValidator : AbstractValidator<ReadLanguageDictionaryRequest>
{
    public ReadLanguageDictionaryRequestValidator()
    {
        RuleFor(d => d.Id)
            .GreaterThan(0)
            .WithMessage("ID must be a positive integer.");

        RuleFor(d => d.LanguageID)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
    }
}
