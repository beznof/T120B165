using FluentValidation;
using Languages.Application.Models.Common;

namespace Languages.Application.Models.Inputs.Dictionary;

public sealed class UpdateDictionaryInput : DictionaryInput
{
    public Optional<string?> Name { get; init; }
}

public class UpdateDictionaryInputValidator : AbstractValidator<UpdateDictionaryInput>
{
    public UpdateDictionaryInputValidator()
    {
        Include(new DictionaryInputValidator());

        When(d => d.Name.IsProvided, () =>
        {
            RuleFor(d => d.Name.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty.")
                .MinimumLength(5)
                .WithMessage("Name should be at least 5 characters long.")
                .MaximumLength(20)
                .WithMessage("Name cannot exceed 20 characters.");
        });

    }
}
