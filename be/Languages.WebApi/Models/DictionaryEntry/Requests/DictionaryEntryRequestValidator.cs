using FluentValidation;

namespace Languages.WebApi.Models.DictionaryEntry.Requests;

public class DictionaryEntryRequestValidator : AbstractValidator<DictionaryEntryRequest>
{
    public DictionaryEntryRequestValidator()
    {
        Include(new BaseDictionaryEntryRequestValidator());
        
        RuleFor(e => e.DictionaryEntryId)
            .GreaterThan(0)
            .WithMessage("DictionaryEntry ID must be a positive integer.");
    }
}