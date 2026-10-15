using System.ComponentModel.DataAnnotations;

namespace Languages.WebApi.Models.Requests.Dictionary;

public class BaseDictionaryRequest
{
    /// <summary>
    /// The language's identifier.
    /// Must be a positive integer.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Language ID is required.")]
    public int? LanguageId { get; set; }
}
