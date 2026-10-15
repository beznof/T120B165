using System.ComponentModel.DataAnnotations;
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
    [Required(ErrorMessage = "Text is required.")]
    public string Text { get; set; } = null!;

    /// <summary>
    /// The translation of the word or phrase.
    /// Required.
    /// Cannot exceed 500 characters.
    /// </summary>
    /// <example>Battle droid</example>
    [Required(ErrorMessage = "Translation is required.")]
    public string Translation { get; set; } = null!;

    /// <summary>
    /// The type of entry.
    /// Required.
    /// Must use 0 for Word type and 1 for Phrase type.
    /// </summary>
    /// <example>0</example>
    [Required(ErrorMessage = "Type is required.")]
    public DictionaryEntryType? Type { get; set; }

    /// <summary>
    /// The phonetic transcription.
    /// Optional.
    /// Cannot exceed 500 characters.
    /// </summary>
    /// <example>ˈklæŋ.kər</example>
    public string? PhoneticTranscription { get; set; }
}
