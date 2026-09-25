namespace Languages.Domain.Entities;

public sealed class Language : BaseEntity
{
    public required string Name { get; set; }
    public string? BackgroundImageUrl { get; set; }
    
    public int? LanguageFamilyID { get; set; }

    public LanguageFamily? LanguageFamily { get; set; }
    public ICollection<LanguageDictionary> Dictionaries { get; set; } = [];
    public ICollection<LanguageLevel> LanguageLevels { get; set; } = [];
}
