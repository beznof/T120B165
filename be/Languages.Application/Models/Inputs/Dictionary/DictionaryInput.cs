using FluentValidation;

namespace Languages.Application.Models.Inputs.Dictionary;

public class DictionaryInput : BaseDictionaryInput
{
    public required int DictionaryId { get; init; }
}

public class DictionaryInputValidator : AbstractValidator<DictionaryInput>
{
    public DictionaryInputValidator()
    {
        Include(new BaseDictionaryInputValidator());

        RuleFor(r => r.DictionaryId)
            .GreaterThan(0)
            .WithMessage("Dictionary ID must be a positive integer.");
    }
}
