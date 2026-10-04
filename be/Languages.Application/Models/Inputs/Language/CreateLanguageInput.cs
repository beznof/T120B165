using FluentValidation;

namespace Languages.Application.Models.Inputs.Language;

public class CreateLanguageInput
{
    public required string Name { get; init; }
}

public class CreateLanguageInputValidator : AbstractValidator<CreateLanguageInput>
{
    public CreateLanguageInputValidator()
    {
        RuleFor(l => l.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");
    }
}
