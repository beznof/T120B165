using FluentValidation;
using Languages.WebApi.Models.Common;
using Languages.Domain.Enums;

namespace Languages.WebApi.Models.Requests.DictionaryEntry;

public sealed class ReadManyDictionaryEntriesRequest : ReadManyRequest
{
    public DictionaryEntryType? Type { get; set; }
}

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

