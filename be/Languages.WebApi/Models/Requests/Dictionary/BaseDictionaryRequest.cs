namespace Languages.WebApi.Models.Requests.Dictionary;

public class BaseDictionaryRequest
{
    /// <summary>
    /// The language's identifier.
    /// Must be a positive integer.
    /// </summary>
    /// <example>1</example>
    public required int LanguageId { get; set; }
}
