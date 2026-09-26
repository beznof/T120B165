using Languages.Domain.Enums;

namespace Languages.WebApi.Models.DictionaryEntry.Requests.Create;

public sealed class CreateDictionaryEntryRequest
{
    public required string Text { get; set; }
    public required string Translation { get; set; }
    public required DictionaryEntryType Type { get; set; }
    public string? PhoneticTranscription { get; set; }
    public required int DictionaryId { get; set; }
    public List<string> Synonyms { get; set; } = [];
}
