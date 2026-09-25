namespace Languages.Domain.Entities;

public sealed class LanguageDictionary : BaseEntity
{
    public required string Name { get; set; }
    public LanguageLevel? Level { get; set; }
    public string? BackgroundImageUrl { get; set; }
    
    public int LanguageID { get; set; }
    public int? LevelID { get; set; }

    public Language Language { get; set; } = null!;
    public ICollection<DictionaryEntry> Entries { get; set; } = [];
}
