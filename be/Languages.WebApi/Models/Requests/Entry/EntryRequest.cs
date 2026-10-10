namespace Languages.WebApi.Models.Requests.Entry;

public sealed class EntryRequest : BaseEntryRequest
{
    /// <summary>
    /// The entry's identifier.
    /// Must be a positive integer.
    /// </summary>
    /// <example>1</example>
    public required int EntryId { get; set; }
}
