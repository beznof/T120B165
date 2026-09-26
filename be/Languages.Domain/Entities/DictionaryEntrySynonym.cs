namespace Languages.Domain.Entities;

public sealed class DictionaryEntrySynonym : BaseEntity
{
    public required string Text { get; set; }
    
    public int DictionaryEntryId { get; set; }
}
