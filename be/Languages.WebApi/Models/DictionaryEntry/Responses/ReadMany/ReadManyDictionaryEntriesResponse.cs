using Languages.WebApi.Models.DictionaryEntry.Responses.Read;

namespace Languages.WebApi.Models.DictionaryEntry.Responses.ReadMany;

public sealed record ReadManyDictionaryEntriesResponse(
    IReadOnlyList<ReadDictionaryEntryResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
