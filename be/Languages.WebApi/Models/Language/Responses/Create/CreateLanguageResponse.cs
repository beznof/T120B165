using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Language.Responses.Create;

public sealed record CreateLanguageResponse(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    LanguageFamilySummaryResponse? LanguageFamily
);
