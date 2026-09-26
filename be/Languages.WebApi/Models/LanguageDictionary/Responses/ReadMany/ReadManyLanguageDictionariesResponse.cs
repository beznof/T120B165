using Languages.WebApi.Models.LanguageDictionary.Responses.Read;

namespace Languages.WebApi.Models.LanguageDictionary.Responses.ReadMany;

public sealed record ReadManyLanguageDictionariesResponse(
    IReadOnlyList<ReadLanguageDictionaryResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
