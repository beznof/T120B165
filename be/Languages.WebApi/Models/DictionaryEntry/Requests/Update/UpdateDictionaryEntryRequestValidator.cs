using FluentValidation;

namespace Languages.WebApi.Models.DictionaryEntry.Requests.Update;

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
                .WithMessage("Text cannot exceed 500 characters.")
                .OverridePropertyName(nameof(UpdateDictionaryEntryRequest.Text));
        });

        When(e => e.Translation.IsSpecified, () =>
        {
            RuleFor(e => e.Translation.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Translation cannot be null or empty.")
                .MaximumLength(500)
                .WithMessage("Translation cannot exceed 500 characters.")
                .OverridePropertyName(nameof(UpdateDictionaryEntryRequest.Translation));
        });

        When(e => e.Type.IsSpecified, () =>
        {
            RuleFor(e => e.Type.Value)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Entry type cannot be null.")
                .IsInEnum()
                .WithMessage("Entry type must be Word or Phrase.")
                .OverridePropertyName(nameof(UpdateDictionaryEntryRequest.Type));
        });

        When(e => e.PhoneticTranscription.IsSpecified, () =>
        {
            RuleFor(e => e.PhoneticTranscription.Value)
                .MaximumLength(500)
                .WithMessage("Phonetic transcription cannot exceed 500 characters.")
                .OverridePropertyName(nameof(UpdateDictionaryEntryRequest.PhoneticTranscription));
        });

        When(e => e.Synonyms.IsSpecified, () =>
        {
            RuleFor(e => e.Synonyms.Value)
                .NotNull()
                .WithMessage("Synonyms cannot be null. Use an empty list to clear them.")
                .OverridePropertyName(nameof(UpdateDictionaryEntryRequest.Synonyms));

            When(e => e.Synonyms.Value != null, () =>
            {
                RuleForEach(e => e.Synonyms.Value)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage("Synonym text is required.")
                    .MaximumLength(500)
                    .WithMessage("Synonym text cannot exceed 500 characters.")
                    .OverridePropertyName(nameof(UpdateDictionaryEntryRequest.Synonyms));
            });
        });
    }
}
