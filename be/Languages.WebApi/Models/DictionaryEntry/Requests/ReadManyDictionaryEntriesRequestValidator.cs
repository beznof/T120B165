using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.DictionaryEntry.Requests;

public class ReadManyDictionaryEntriesRequestValidator : AbstractValidator<ReadManyDictionaryEntriesRequest>
{
    public ReadManyDictionaryEntriesRequestValidator()
    {
        Include(new ReadManyRequestValidator());

        When(e => e.Type != null, () =>
        {
            RuleFor(e => e.Type)
                .IsInEnum()
                .WithMessage("Entry type must be Word or Phrase.");
        });
    }
}
