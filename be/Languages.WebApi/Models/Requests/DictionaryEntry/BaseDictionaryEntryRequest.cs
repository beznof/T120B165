using FluentValidation;

namespace Languages.WebApi.Models.Requests.DictionaryEntry;

public class BaseDictionaryEntryRequest
{
    public required int LanguageId { get; set; }
    public required int DictionaryId { get; set; }
}

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