using System.ComponentModel.DataAnnotations;

namespace Languages.WebApi.Models.Requests.Entry;

public class BaseEntryRequest
{
    /// <summary>
    /// The language's identifier.
    /// Must be a positive integer.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Language ID is required.")]
    public int? LanguageId { get; set; }

    /// <summary>
    /// The dictionary's identifier.
    /// Must be a positive integer.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Dictionary ID is required.")]
    public int? DictionaryId { get; set; }
}
