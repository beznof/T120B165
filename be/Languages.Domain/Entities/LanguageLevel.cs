namespace Languages.Domain.Entities;

public sealed class LanguageLevel : BaseEntity
{
    public required string Name { get; set; }
    
    public int LanguageID { get; set; }
}
