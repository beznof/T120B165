namespace Languages.WebApi.Models.Language.Requests;

public sealed class SetLanguageBackgroundImageRequest
{
    public IFormFile BackgroundImage { get; set; }
}
