using Languages.Application.Models.Common;

namespace Languages.WebApi.Models.Requests.Dictionary;

public sealed class UpdateDictionaryRequest
{
    /// <summary>
    /// The name of the dictionary.
    /// Must be omitted or non-null.
    /// If supplied, it cannot be empty and must be between 5 and 20 characters long.
    /// </summary>
    /// <example>Common words</example>
    public Optional<string?> Name { get; set; }
}
