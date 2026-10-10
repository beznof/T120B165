namespace Languages.WebApi.Models.Requests.Dictionary;

public class DictionaryRequest : BaseDictionaryRequest
{
    /// <summary>
    /// The dictionary's identifier.
    /// Must be a positive integer.
    /// </summary>
    /// <example>1</example>
    public required int DictionaryId { get; set; }
}
