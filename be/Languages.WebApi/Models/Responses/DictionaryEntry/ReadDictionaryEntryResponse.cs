using Languages.Domain.Enums;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Responses.DictionaryEntry;

public sealed record ReadDictionaryEntryResponse(
    int Id,
    string Text,
    string Translation,
    DictionaryEntryType Type,
    string? PhoneticTranscription,
    LanguageDictionarySummaryResponse Dictionary
);
