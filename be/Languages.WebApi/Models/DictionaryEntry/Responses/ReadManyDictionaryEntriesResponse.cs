
namespace Languages.WebApi.Models.DictionaryEntry.Responses;

public sealed record ReadManyDictionaryEntriesResponse(
    IReadOnlyList<ReadDictionaryEntryResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
