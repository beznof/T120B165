using Languages.WebApi.Models.Requests.Common;
using Languages.Domain.Enums;

namespace Languages.WebApi.Models.Requests.Entry;

public sealed class GetManyEntriesRequest : GetManyRequest
{
    public DictionaryEntryType? Type { get; set; }
}
