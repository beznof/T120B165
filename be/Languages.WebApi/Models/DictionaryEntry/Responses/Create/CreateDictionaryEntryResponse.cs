using Languages.WebApi.Models.Common;
using Languages.Domain.Enums;

namespace Languages.WebApi.Models.DictionaryEntry.Responses.Create;

public sealed record CreateDictionaryEntryResponse(
    int Id,
    string Text,
    string Translation,
    DictionaryEntryType Type,
    string? PhoneticTranscription,
    LanguageDictionarySummaryResponse Dictionary,
    IReadOnlyList<string> Synonyms
);
