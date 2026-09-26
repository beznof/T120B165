namespace Languages.WebApi.Models.LanguageDictionary.Requests.Delete;

public sealed class DeleteLanguageDictionaryRequest
{
    public required int Id { get; set; }
    public required int LanguageID { get; set; }
}
