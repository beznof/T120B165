using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Language.Responses.Read;

public sealed record ReadLanguageResponse(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    LanguageFamilySummaryResponse? LanguageFamily
);
