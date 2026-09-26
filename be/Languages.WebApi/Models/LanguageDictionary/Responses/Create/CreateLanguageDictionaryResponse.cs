using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageDictionary.Responses.Create;

public sealed record CreateLanguageDictionaryResponse(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    LanguageSummaryResponse Language
);
