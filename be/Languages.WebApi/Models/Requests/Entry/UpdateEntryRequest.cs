using Languages.Application.Models.Common;
using Languages.Domain.Enums;

namespace Languages.WebApi.Models.Requests.Entry;

public sealed class UpdateEntryRequest
{
    /// <summary>
    /// The phrase or word text.
    /// Must be omitted or non-null.
    /// If supplied, it cannot be empty or exceed 500 characters.
    /// </summary>
    /// <example>Clanker</example>
    public Optional<string?> Text { get; set; }

    /// <summary>
    /// The translation of the word or phrase.
    /// Must be omitted or non-null.
    /// If supplied, it cannot be empty or exceed 500 characters.
    /// </summary>
    /// <example>Battle droid</example>
    public Optional<string?> Translation { get; set; }

    /// <summary>
    /// The type of entry.
    /// Must be omitted or non-null.
    /// If supplied, must use 0 for Word type and 1 for Phrase type.
    /// </summary>
    /// <example>0</example>
    public Optional<DictionaryEntryType?> Type { get; set; }

    /// <summary>
    /// The phonetic transcription.
    /// May be omitted or null.
    /// If supplied, it cannot exceed 500 characters.
    /// Set to null or an empty string for removal.
    /// </summary>
    /// <example>ˈklæŋ.kər</example>
    public Optional<string?> PhoneticTranscription { get; set; }
}
