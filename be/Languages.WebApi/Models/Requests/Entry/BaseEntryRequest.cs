namespace Languages.WebApi.Models.Requests.Entry;

public class BaseEntryRequest
{
    public required int LanguageId { get; set; }
    public required int DictionaryId { get; set; }
}
