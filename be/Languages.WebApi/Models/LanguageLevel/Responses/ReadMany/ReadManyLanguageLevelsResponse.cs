using Languages.WebApi.Models.LanguageLevel.Responses.Read;

namespace Languages.WebApi.Models.LanguageLevel.Responses.ReadMany;

public sealed record ReadManyLanguageLevelsResponse(
    IReadOnlyList<ReadLanguageLevelResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
