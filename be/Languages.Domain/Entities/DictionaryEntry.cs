using Languages.Domain.Enums;

namespace Languages.Domain.Entities;

public sealed class DictionaryEntry : BaseEntity
{
    public required string Text { get; set; }
    public required string Translation { get; set; }
    public required DictionaryEntryType Type { get; set; }
    public string? PhoneticTranscription { get; set; }
    
    public int DictionaryID { get; set; }

    public LanguageDictionary Dictionary { get; set; } = null!;
    public ICollection<DictionaryEntrySynonym> Synonyms { get; set; } = [];
}
