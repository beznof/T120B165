using System.ComponentModel.DataAnnotations;

namespace Languages.WebApi.Models.Requests.Dictionary;

public class DictionaryRequest : BaseDictionaryRequest
{
    /// <summary>
    /// The dictionary's identifier.
    /// Must be a positive integer.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Dictionary ID is required.")]
    public int? DictionaryId { get; set; }
}
