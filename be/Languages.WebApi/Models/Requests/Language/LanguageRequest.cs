namespace Languages.WebApi.Models.Requests.Language;

public class LanguageRequest
{
    /// <summary>
    /// The language's identifier.
    /// Must be a positive integer.
    /// </summary>
    /// <example>1</example>
    public required int LanguageId { get; set; }
}
