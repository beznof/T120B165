using FluentValidation;

namespace Languages.WebApi.Models.DictionaryEntry.Requests.Create;

public class CreateDictionaryEntryRequestValidator : AbstractValidator<CreateDictionaryEntryRequest>
{
    public CreateDictionaryEntryRequestValidator()
    {
        RuleFor(e => e.DictionaryId)
            .GreaterThan(0)
            .WithMessage("Dictionary ID must be a positive integer.");

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

        RuleFor(e => e.PhoneticTranscription)
            .MaximumLength(500)
            .WithMessage("Phonetic transcription cannot exceed 500 characters.");

        RuleFor(e => e.Synonyms)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Synonyms must be a list, even when empty.")
            .Must(synonyms => synonyms
                .Select(s => s?.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count() == synonyms.Count)
            .WithMessage("Synonyms must not contain duplicates.");

        When(e => e.Synonyms != null, () =>
        {
            RuleForEach(e => e.Synonyms)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Synonym text is required.")
                .MaximumLength(500)
                .WithMessage("Synonym text cannot exceed 500 characters.");
        });
    }
}
