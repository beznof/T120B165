using FluentValidation;
using Languages.Application.Models.Inputs.Common;
using Languages.Domain.Enums;

namespace Languages.Application.Models.Inputs.Entry;

public sealed class GetManyEntriesInput : BaseEntryInput
{
    public required GetManyInput Pagination { get; init; }
    public DictionaryEntryType? Type { get; init; }
}

public class GetManyEntriesInputValidator : AbstractValidator<GetManyEntriesInput>
{
    public GetManyEntriesInputValidator()
    {
        Include(new BaseEntryInputValidator());

        RuleFor(l => l.Pagination)
            .NotNull()
            .SetValidator(new GetManyInputValidator());

        When(e => e.Type != null, () =>
        {
            RuleFor(e => e.Type)
                .IsInEnum()
                .WithMessage("Entry type must be Word or Phrase.");
        });
    }
}
