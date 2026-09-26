using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageDictionary.Responses.Read;

public sealed record ReadLanguageDictionaryResponse(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    LanguageSummaryResponse Language
);
