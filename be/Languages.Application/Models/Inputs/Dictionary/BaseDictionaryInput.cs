using FluentValidation;

namespace Languages.Application.Models.Inputs.Dictionary;

public class BaseDictionaryInput
{
    public required int LanguageId { get; init; }
}

public class BaseDictionaryInputValidator : AbstractValidator<BaseDictionaryInput>
{
    public BaseDictionaryInputValidator()
    {
        RuleFor(r => r.LanguageId)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
    }
}
