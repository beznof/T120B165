namespace Languages.WebApi.Models.Language.Requests.UpdateBackgroundImage;

// Bind from multipart form data; file uploads do not use the JSON patch converter.
public sealed class UpdateLanguageBackgroundImageRequest
{
    public IFormFile? BackgroundImage { get; set; }
    public bool RemoveBackgroundImage { get; set; }
}
