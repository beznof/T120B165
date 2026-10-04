using FluentValidation;

namespace Languages.Application.Models.Inputs.Entry;

public class EntryInput : BaseEntryInput
{
    public required int EntryId { get; init; }
}

public class EntryInputValidator : AbstractValidator<EntryInput>
{
    public EntryInputValidator()
    {
        Include(new BaseEntryInputValidator());

        RuleFor(e => e.EntryId)
            .GreaterThan(0)
            .WithMessage("DictionaryEntry ID must be a positive integer.");
    }
}
