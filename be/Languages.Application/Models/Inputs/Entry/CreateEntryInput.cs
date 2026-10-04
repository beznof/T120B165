using FluentValidation;
using Languages.Domain.Enums;

namespace Languages.Application.Models.Inputs.Entry;

public sealed class CreateEntryInput : BaseEntryInput
{
    public required string Text { get; init; }
    public required string Translation { get; init; }
    public required DictionaryEntryType Type { get; init; }
    public string? PhoneticTranscription { get; init; } = null;
}

public class CreateEntryInputValidator : AbstractValidator<CreateEntryInput>
{
    public CreateEntryInputValidator()
    {
        Include(new BaseEntryInputValidator());

        RuleFor(e => e.Text)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Text is required.")
            .MaximumLength(500)
            .WithMessage("Text cannot exceed 500 characters.");

        RuleFor(e => e.Translation)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Translation is required.")
            .MaximumLength(500)
            .WithMessage("Translation cannot exceed 500 characters.");

        RuleFor(e => e.Type)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Entry type is required.")
            .IsInEnum()
            .WithMessage("Entry type must be Word or Phrase.");

        When(e => e.PhoneticTranscription != null, () =>
        {
            RuleFor(e => e.PhoneticTranscription)
                .MaximumLength(500)
                .WithMessage("Phonetic transcription cannot exceed 500 characters.");
        });
    }
}
