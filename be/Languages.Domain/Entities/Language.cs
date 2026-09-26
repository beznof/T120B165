namespace Languages.Domain.Entities;

public sealed class Language : BaseEntity
{
    public required string Name { get; set; }
    public string? BackgroundImageUrl { get; set; }
    
    public ICollection<LanguageDictionary> Dictionaries { get; set; } = [];
}
