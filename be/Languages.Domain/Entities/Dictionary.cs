namespace Languages.Domain.Entities;

public sealed class Dictionary : BaseEntity
{
    public required string Name { get; set; }
    public string? BackgroundImageUrl { get; set; }
    
    public int LanguageId { get; set; }

    public Language Language { get; set; } = null!;
    public ICollection<Entry> Entries { get; set; } = [];
}
