using FluentValidation;

namespace Languages.Application.Models.Inputs.Entry;

public class BaseEntryInput
{
    public required int LanguageId { get; init; }
    public required int DictionaryId { get; init; }
}

public class BaseEntryInputValidator : AbstractValidator<BaseEntryInput>
{
    public BaseEntryInputValidator()
    {
        RuleFor(e => e.LanguageId)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");

        RuleFor(e => e.DictionaryId)
            .GreaterThan(0)
            .WithMessage("Dictionary ID must be a positive integer.");
    }
}
