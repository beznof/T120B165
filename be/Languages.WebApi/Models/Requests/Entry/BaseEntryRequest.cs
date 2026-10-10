namespace Languages.WebApi.Models.Requests.Entry;

public class BaseEntryRequest
{
    /// <summary>
    /// The language's identifier.
    /// Must be a positive integer.
    /// </summary>
    /// <example>1</example>
    public required int LanguageId { get; set; }

    /// <summary>
    /// The dictionary's identifier.
    /// Must be a positive integer.
    /// </summary>
    /// <example>1</example>
    public required int DictionaryId { get; set; }
}
