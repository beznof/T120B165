using Languages.Domain.Enums;

namespace Languages.Application.Models.DictionaryEntry;

public sealed class CreateDictionaryEntryInput
{
    public required string Text { get; init; }
    public required string Translation { get; init; }
    public required DictionaryEntryType Type { get; init; }
    public string? PhoneticTranscription { get; init; }
}
