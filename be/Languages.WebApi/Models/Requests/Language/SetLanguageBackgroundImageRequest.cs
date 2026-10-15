using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Languages.WebApi.Models.Requests.Language;

public sealed class SetLanguageBackgroundImageRequest
{
    /// <summary>
    /// The background image.
    /// Required.
    /// Must not be empty.
    /// Must not exceed 5 MiB in size.
    /// Must be at least 500px wide and 500px tall.
    /// Must be in JPEG or PNG format.
    /// </summary>
    [Required(ErrorMessage = "Background image is required.")]
    public IFormFile? BackgroundImage { get; set; }
}
