using FluentValidation;

namespace Languages.WebApi.Models.DictionaryEntry.Requests;

public class UpdateDictionaryEntryRequestValidator : AbstractValidator<UpdateDictionaryEntryRequest>
{
    public UpdateDictionaryEntryRequestValidator()
    {
        When(e => e.Text.IsSpecified, () =>
        {
            RuleFor(e => e.Text.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Text cannot be null or empty.")
                .MaximumLength(500)
                .WithMessage("Text cannot exceed 500 characters.");
        });

        When(e => e.Translation.IsSpecified, () =>
        {
            RuleFor(e => e.Translation.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Translation cannot be null or empty.")
                .MaximumLength(500)
                .WithMessage("Translation cannot exceed 500 characters.");
        });

        When(e => e.Type.IsSpecified, () =>
        {
            RuleFor(e => e.Type.Value)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Entry type cannot be null.")
                .IsInEnum()
                .WithMessage("Entry type must be Word or Phrase.");
        });

        When(e => e.PhoneticTranscription.IsSpecified, () =>
        {
            RuleFor(e => e.PhoneticTranscription.Value)
                .MaximumLength(500)
                .WithMessage("Phonetic transcription cannot exceed 500 characters.");
        });
    }
}
