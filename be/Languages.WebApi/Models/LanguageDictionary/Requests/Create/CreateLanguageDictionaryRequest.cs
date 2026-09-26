namespace Languages.WebApi.Models.LanguageDictionary.Requests.Create;

public sealed class CreateLanguageDictionaryRequest
{
    public required string Name { get; set; }
    public required int LanguageID { get; set; }
    public IFormFile? BackgroundImage { get; set; }
    public int? LevelID { get; set; }
}
