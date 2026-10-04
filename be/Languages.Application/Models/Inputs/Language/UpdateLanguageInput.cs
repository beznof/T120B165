using FluentValidation;
using Languages.Application.Models.Common;

namespace Languages.Application.Models.Inputs.Language;

public class UpdateLanguageInput : LanguageInput
{
    public Optional<string?> Name { get; init; }
}

public class UpdateLanguageInputValidator : AbstractValidator<UpdateLanguageInput>
{
    public UpdateLanguageInputValidator()
    {
        Include(new LanguageInputValidator());

        When(l => l.Name.IsProvided, () =>
        {
            RuleFor(l => l.Name.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty.")
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.")
                .OverridePropertyName(nameof(UpdateLanguageInput.Name));
        });

    }
}
