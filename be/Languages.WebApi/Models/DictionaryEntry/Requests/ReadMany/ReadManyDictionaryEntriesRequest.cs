using Languages.WebApi.Models.Common;
using Languages.Domain.Enums;

namespace Languages.WebApi.Models.DictionaryEntry.Requests.ReadMany;

public sealed class ReadManyDictionaryEntriesRequest : ReadManyRequest
{
    public int DictionaryID { get; set; }
    public DictionaryEntryType? Type { get; set; }
}
