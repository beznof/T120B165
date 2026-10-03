namespace Languages.WebApi.Models.LanguageDictionary.Requests;

public class LanguageDictionaryRequest : BaseLanguageDictionaryRequest
{
    public required int DictionaryId { get; set; }
}