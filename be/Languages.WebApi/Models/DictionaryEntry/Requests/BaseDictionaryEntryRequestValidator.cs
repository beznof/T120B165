using FluentValidation;

namespace Languages.WebApi.Models.DictionaryEntry.Requests;

public class BaseDictionaryEntryRequestValidator : AbstractValidator<BaseDictionaryEntryRequest>
{
    public BaseDictionaryEntryRequestValidator()
    {
        RuleFor(e => e.LanguageId)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
        
        RuleFor(e => e.DictionaryId)
            .GreaterThan(0)
            .WithMessage("Dictionary ID must be a positive integer.");
    }
}