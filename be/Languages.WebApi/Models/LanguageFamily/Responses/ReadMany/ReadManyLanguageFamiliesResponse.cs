using Languages.WebApi.Models.LanguageFamily.Responses.Read;

namespace Languages.WebApi.Models.LanguageFamily.Responses.ReadMany;

public sealed record ReadManyLanguageFamiliesResponse(
    IReadOnlyList<ReadLanguageFamilyResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
