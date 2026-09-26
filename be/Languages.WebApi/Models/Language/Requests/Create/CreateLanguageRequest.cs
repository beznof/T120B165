namespace Languages.WebApi.Models.Language.Requests.Create;

public sealed class CreateLanguageRequest
{
    public required string Name { get; set; }
    public IFormFile? BackgroundImage { get; set; }
    public int? LanguageFamilyID { get; set; }
}
