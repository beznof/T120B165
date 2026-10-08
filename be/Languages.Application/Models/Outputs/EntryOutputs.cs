using Languages.Domain.Enums;

namespace Languages.Application.Models.Outputs;

public sealed record CreateEntryOutput(
    int Id,
    string Text,
    string Translation,
    DictionaryEntryType Type,
    string? PhoneticTranscription,
    DictionarySummaryOutput Dictionary
);

public sealed record GetOneEntryOutput(
    int Id,
    string Text,
    string Translation,
    DictionaryEntryType Type,
    string? PhoneticTranscription,
    DictionarySummaryOutput Dictionary
);

public sealed record UpdateEntryOutput(
    int Id,
    string Text,
    string Translation,
    DictionaryEntryType Type,
    string? PhoneticTranscription,
    DictionarySummaryOutput Dictionary
);

public sealed record GetManyEntriesOutput(
    IReadOnlyList<GetOneEntryOutput> Items,
    int Page,
    int PageSize,
    int TotalCount
) : GetManyOutput(Page, PageSize, TotalCount);
