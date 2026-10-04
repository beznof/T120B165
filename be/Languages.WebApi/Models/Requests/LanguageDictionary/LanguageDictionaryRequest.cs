namespace Languages.WebApi.Models.Requests.LanguageDictionary;

public class LanguageDictionaryRequest : BaseLanguageDictionaryRequest
{
    public required int DictionaryId { get; set; }
}
