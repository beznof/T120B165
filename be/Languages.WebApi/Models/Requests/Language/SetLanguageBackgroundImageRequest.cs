using FluentValidation;

namespace Languages.WebApi.Models.Requests.Language;

public sealed class SetLanguageBackgroundImageRequest
{
    /// <summary>
    /// The background image.
    /// Required.
    /// Must not be empty.
    /// Must not exceed 5 MiB in size.
    /// Must be in JPEG or PNG format.
    /// </summary>
    public IFormFile BackgroundImage { get; set; }
}
