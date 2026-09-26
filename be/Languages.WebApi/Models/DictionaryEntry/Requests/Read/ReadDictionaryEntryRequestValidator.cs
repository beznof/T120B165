using FluentValidation;

namespace Languages.WebApi.Models.DictionaryEntry.Requests.Read;

public class ReadDictionaryEntryRequestValidator : AbstractValidator<ReadDictionaryEntryRequest>
{
    public ReadDictionaryEntryRequestValidator()
    {
        RuleFor(e => e.Id)
            .GreaterThan(0)
            .WithMessage("ID must be a positive integer.");

        RuleFor(e => e.DictionaryID)
            .GreaterThan(0)
            .WithMessage("Dictionary ID must be a positive integer.");
    }
}
