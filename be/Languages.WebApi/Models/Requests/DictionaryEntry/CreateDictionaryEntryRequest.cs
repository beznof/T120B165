using FluentValidation;
using Languages.Domain.Enums;

namespace Languages.WebApi.Models.Requests.DictionaryEntry;

public sealed class CreateDictionaryEntryRequest
{
    public required string Text { get; set; }
    public required string Translation { get; set; }
    public required DictionaryEntryType Type { get; set; }
    public string? PhoneticTranscription { get; set; }
}

public class CreateDictionaryEntryRequestValidator : AbstractValidator<CreateDictionaryEntryRequest>
{
    public CreateDictionaryEntryRequestValidator()
    {
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
    }
}