namespace Languages.WebApi.Models.LanguageDictionary.Responses;

public sealed record ReadManyLanguageDictionariesResponse(
    IReadOnlyList<ReadLanguageDictionaryResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
