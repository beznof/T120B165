using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.DictionaryEntry.Requests.ReadMany;

public class ReadManyDictionaryEntriesRequestValidator : AbstractValidator<ReadManyDictionaryEntriesRequest>
{
    public ReadManyDictionaryEntriesRequestValidator()
    {
        Include(new ReadManyRequestValidator());
        
        RuleFor(e => e.DictionaryID)
            .GreaterThan(0)
            .WithMessage("Dictionary ID must be a positive integer.");

        When(e => e.Type != null, () =>
        {
            RuleFor(e => e.Type)
                .IsInEnum()
                .WithMessage("Entry type must be Word or Phrase.");
        });
    }
}
