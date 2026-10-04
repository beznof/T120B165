using FluentValidation;

namespace Languages.Application.Models.Inputs.Language;

public class LanguageInput
{
    public required int LanguageId { get; init; }
}

public class LanguageInputValidator : AbstractValidator<LanguageInput>
{
    public LanguageInputValidator()
    {
        RuleFor(e => e.LanguageId)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
    }
}
