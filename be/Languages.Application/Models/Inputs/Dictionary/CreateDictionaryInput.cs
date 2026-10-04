using FluentValidation;

namespace Languages.Application.Models.Inputs.Dictionary;

public sealed class CreateDictionaryInput : BaseDictionaryInput
{
    public required string Name { get; init; }
}

public class CreateDictionaryInputValidator : AbstractValidator<CreateDictionaryInput>
{
    public CreateDictionaryInputValidator()
    {
        Include(new BaseDictionaryInputValidator());

        RuleFor(d => d.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(5)
            .WithMessage("Name should be at least 5 characters long.")
            .MaximumLength(20)
            .WithMessage("Name cannot exceed 20 characters.");
    }
}
