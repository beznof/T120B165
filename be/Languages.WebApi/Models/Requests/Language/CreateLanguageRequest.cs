using System.ComponentModel.DataAnnotations;

namespace Languages.WebApi.Models.Requests.Language;

public sealed class CreateLanguageRequest
{
    /// <summary>
    /// The name of the language.
    /// Required.
    /// Cannot exceed 100 characters.
    /// </summary>
    /// <example>Ewokese</example>
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = null!;
}
