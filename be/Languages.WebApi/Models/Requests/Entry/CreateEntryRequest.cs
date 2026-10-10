using Languages.Domain.Enums;

namespace Languages.WebApi.Models.Requests.Entry;

public sealed class CreateEntryRequest
{
    /// <summary>
    /// The phrase or word text.
    /// Required.
    /// Cannot exceed 500 characters.
    /// </summary>
    /// <example>Clanker</example>
    public required string Text { get; set; }

    /// <summary>
    /// The translation of the word or phrase.
    /// Required.
    /// Cannot exceed 500 characters.
    /// </summary>
    /// <example>Battle droid</example>
    public required string Translation { get; set; }

    /// <summary>
    /// The type of entry.
    /// Required.
    /// Must use 0 for Word type and 1 for Phrase type.
    /// </summary>
    /// <example>0</example>
    public required DictionaryEntryType Type { get; set; }

    /// <summary>
    /// The phonetic transcription.
    /// Optional.
    /// Cannot exceed 500 characters.
    /// </summary>
    /// <example>ˈklæŋ.kər</example>
    public string? PhoneticTranscription { get; set; }
}
