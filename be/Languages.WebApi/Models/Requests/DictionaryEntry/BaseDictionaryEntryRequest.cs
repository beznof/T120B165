namespace Languages.WebApi.Models.Requests.DictionaryEntry;

public class BaseDictionaryEntryRequest
{
    public required int LanguageId { get; set; }
    public required int DictionaryId { get; set; }
}
