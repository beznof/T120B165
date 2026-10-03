namespace Languages.WebApi.Models.DictionaryEntry.Requests;

public class BaseDictionaryEntryRequest
{
    public required int LanguageId { get; set; }
    public required int DictionaryId { get; set; }
}