using FluentValidation;
using Languages.Application.Models.Common;
using Languages.Domain.Enums;

namespace Languages.Application.Models.Inputs.Entry;

public sealed class UpdateEntryInput : EntryInput
{
    public Optional<string?> Text { get; init; }
    public Optional<string?> Translation { get; init; }
    public Optional<DictionaryEntryType?> Type { get; init; }
    public Optional<string?> PhoneticTranscription { get; init; }
}

public class UpdateEntryInputValidator : AbstractValidator<UpdateEntryInput>
{
    public UpdateEntryInputValidator()
    {
        Include(new EntryInputValidator());

        When(e => e.Text.IsProvided, () =>
        {
            RuleFor(e => e.Text.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Text cannot be null or empty.")
                .MaximumLength(500)
                .WithMessage("Text cannot exceed 500 characters.");
        });

        When(e => e.Translation.IsProvided, () =>
        {
            RuleFor(e => e.Translation.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Translation cannot be null or empty.")
                .MaximumLength(500)
                .WithMessage("Translation cannot exceed 500 characters.");
        });

        When(e => e.Type.IsProvided, () =>
        {
            RuleFor(e => e.Type.Value)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Entry type cannot be null.")
                .IsInEnum()
                .WithMessage("Entry type must be Word or Phrase.");
        });

        When(e => e.PhoneticTranscription.IsProvided, () =>
        {
            RuleFor(e => e.PhoneticTranscription.Value)
                .MaximumLength(500)
                .WithMessage("Phonetic transcription cannot exceed 500 characters.");
        });
    }
}
