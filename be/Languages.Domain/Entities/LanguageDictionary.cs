namespace Languages.Domain.Entities;

public sealed class LanguageDictionary : BaseEntity
{
    public required string Name { get; set; }
    public LanguageLevel? LanguageLevel { get; set; }
    public string? BackgroundImageUrl { get; set; }
    
    public int LanguageId { get; set; }
    public int? LevelId { get; set; }

    public Language Language { get; set; } = null!;
    public ICollection<DictionaryEntry> Entries { get; set; } = [];
}
