using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Responses.LanguageDictionary;

public sealed record ReadLanguageDictionaryResponse(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    LanguageSummaryResponse Language
);

