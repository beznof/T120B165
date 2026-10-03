namespace Languages.WebApi.Models.Responses.Language;

public sealed record ReadManyLanguagesResponse(
    IReadOnlyList<ReadLanguageResponse> Items,
    int Page,
    int PageSize,
    int TotalCount
);
