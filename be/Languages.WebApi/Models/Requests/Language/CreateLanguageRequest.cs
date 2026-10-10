namespace Languages.WebApi.Models.Requests.Language;

public sealed class CreateLanguageRequest
{
    /// <summary>
    /// The name of the language.
    /// Required.
    /// Cannot exceed 100 characters.
    /// </summary>
    /// <example>Ewokese</example>
    public required string Name { get; set; }
}
