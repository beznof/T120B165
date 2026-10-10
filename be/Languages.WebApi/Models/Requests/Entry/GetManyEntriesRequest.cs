using Languages.WebApi.Models.Requests.Common;
using Languages.Domain.Enums;

namespace Languages.WebApi.Models.Requests.Entry;

public sealed class GetManyEntriesRequest : GetManyRequest
{
    /// <summary>
    /// Optional entry type for filtering the results.
    /// Must use 0 for Word type and 1 for Phrase type.
    /// </summary>
    /// <example>0</example>
    public DictionaryEntryType? Type { get; set; }
}
