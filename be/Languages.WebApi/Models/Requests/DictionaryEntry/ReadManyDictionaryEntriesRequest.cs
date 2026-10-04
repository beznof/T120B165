using Languages.WebApi.Models.Common;
using Languages.Domain.Enums;

namespace Languages.WebApi.Models.Requests.DictionaryEntry;

public sealed class ReadManyDictionaryEntriesRequest : ReadManyRequest
{
    public DictionaryEntryType? Type { get; set; }
}
