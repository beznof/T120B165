namespace Languages.WebApi.Models.Language.Responses;

public sealed record ReadManyLanguagesResponse(
    IReadOnlyList<ReadLanguageResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
