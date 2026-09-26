using Languages.WebApi.Models.Language.Responses.Read;

namespace Languages.WebApi.Models.Language.Responses.ReadMany;

public sealed record ReadManyLanguagesResponse(
    IReadOnlyList<ReadLanguageResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
