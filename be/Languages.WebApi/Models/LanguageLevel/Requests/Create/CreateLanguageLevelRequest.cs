namespace Languages.WebApi.Models.LanguageLevel.Requests.Create;

public sealed class CreateLanguageLevelRequest
{
    public required string Name { get; set; }
    public required int LanguageID { get; set; }
}
