using FluentValidation;
using Languages.Application.Models.Inputs.Common;

namespace Languages.Application.Models.Inputs.Language;

public sealed class SetLanguageBackgroundImageInput : LanguageInput
{
    public required SetImageInput Image { get; init; }
}

public class SetLanguageBackgroundImageInputValidator : AbstractValidator<SetLanguageBackgroundImageInput>
{
    public SetLanguageBackgroundImageInputValidator()
    {
        Include(new LanguageInputValidator());

        RuleFor(l => l.Image)
            .NotNull()
            .SetValidator(new SetImageInputValidator());
    }
}
