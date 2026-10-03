namespace Languages.WebApi.Models.Responses.DictionaryEntry;

public sealed record ReadManyDictionaryEntriesResponse(
    IReadOnlyList<ReadDictionaryEntryResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
