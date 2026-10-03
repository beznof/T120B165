using FluentValidation;

namespace Languages.WebApi.Models.Requests.DictionaryEntry;

public sealed class DictionaryEntryRequest : BaseDictionaryEntryRequest
{
    public int DictionaryEntryId { get; set; }
}

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