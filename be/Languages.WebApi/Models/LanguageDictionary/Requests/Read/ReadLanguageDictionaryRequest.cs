namespace Languages.WebApi.Models.LanguageDictionary.Requests.Read;

public sealed class ReadLanguageDictionaryRequest
{
    public required int Id { get; set; }
    public required int LanguageID { get; set; }
}
