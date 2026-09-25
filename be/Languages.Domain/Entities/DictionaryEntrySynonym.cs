namespace Languages.Domain.Entities;

public sealed class DictionaryEntrySynonym : BaseEntity
{
    public required string Text { get; set; }
    
    public int DictionaryEntryID { get; set; }
}
