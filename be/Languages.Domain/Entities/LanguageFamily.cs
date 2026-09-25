namespace Languages.Domain.Entities;

public sealed class LanguageFamily : BaseEntity
{
    public required string Name { get; set; }
    
    public ICollection<Language> Languages { get; set; } = [];
}