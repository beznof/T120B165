namespace Languages.WebApi.Models.LanguageLevel.Requests.Delete;

public sealed class DeleteLanguageLevelRequest
{
    public required int Id { get; set; }
    public required int LanguageID { get; set; }
}
