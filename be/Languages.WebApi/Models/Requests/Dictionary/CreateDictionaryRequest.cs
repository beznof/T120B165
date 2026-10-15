using System.ComponentModel.DataAnnotations;

namespace Languages.WebApi.Models.Requests.Dictionary;

public sealed class CreateDictionaryRequest
{
    /// <summary>
    /// The name of the dictionary.
    /// Required.
    /// Must be between 5 and 20 characters long.
    /// </summary>
    /// <example>Common words</example>
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = null!;
}
