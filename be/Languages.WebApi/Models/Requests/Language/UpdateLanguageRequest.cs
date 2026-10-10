using Languages.Application.Models.Common;

namespace Languages.WebApi.Models.Requests.Language;

public sealed class UpdateLanguageRequest
{
    /// <summary>
    /// The name of the language.
    /// Must be omitted or non-null.
    /// If supplied, it cannot be empty and cannot exceed 100 characters.
    /// </summary>
    /// <example>Huttese</example>
    public Optional<string?> Name { get; set; }
}
