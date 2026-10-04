using FluentValidation;
using Languages.Application.Models.Inputs.Common;

namespace Languages.Application.Models.Inputs.Dictionary;

public sealed class SetDictionaryBackgroundImageInput : DictionaryInput
{
    public required SetImageInput Image { get; init; }
}

public class SetDictionaryBackgroundImageInputValidator : AbstractValidator<SetDictionaryBackgroundImageInput>
{
    public SetDictionaryBackgroundImageInputValidator()
    {
        Include(new DictionaryInputValidator());

        RuleFor(d => d.Image)
            .NotNull()
            .SetValidator(new SetImageInputValidator());
    }
}
