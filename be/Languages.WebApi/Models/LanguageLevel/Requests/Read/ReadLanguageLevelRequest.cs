namespace Languages.WebApi.Models.LanguageLevel.Requests.Read;

public sealed class ReadLanguageLevelRequest
{
    public required int Id { get; set; }
    public required int LanguageID { get; set; }
}
