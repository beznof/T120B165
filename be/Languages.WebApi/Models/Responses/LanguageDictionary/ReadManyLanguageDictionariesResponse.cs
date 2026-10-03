namespace Languages.WebApi.Models.Responses.LanguageDictionary;

public sealed record ReadManyLanguageDictionariesResponse(
    IReadOnlyList<ReadLanguageDictionaryResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
