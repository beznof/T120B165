using FluentValidation;

namespace Languages.WebApi.Models.LanguageDictionary.Requests.Delete;

public class DeleteLanguageDictionaryRequestValidator : AbstractValidator<DeleteLanguageDictionaryRequest>
{
    public DeleteLanguageDictionaryRequestValidator()
    {
        RuleFor(d => d.Id)
            .GreaterThan(0)
            .WithMessage("ID must be a positive integer.");

        RuleFor(d => d.LanguageID)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
    }
}
