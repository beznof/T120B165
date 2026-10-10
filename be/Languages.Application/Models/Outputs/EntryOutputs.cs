using Languages.Domain.Enums;

namespace Languages.Application.Models.Outputs;

/// <summary>The created entry.</summary>
/// <param name="Id">The entry's identifier.</param>
/// <param name="Text">The phrase or word text.</param>
/// <param name="Translation">The translation of the word or phrase.</param>
/// <param name="Type">
/// The type of entry.
/// 0 for Word type and 1 for Phrase type.
/// </param>
/// <param name="PhoneticTranscription">The phonetic transcription.</param>
/// <param name="Dictionary">The identifier and name of the dictionary the entry belongs to.</param>
/// <param name="CreatedAtUTC">The date and time when the entry was created, in UTC.</param>
/// <example>
/// {
///   "id": 1,
///   "text": "Clanker",
///   "translation": "Battle droid",
///   "type": 0,
///   "phoneticTranscription": "ˈklæŋ.kər",
///   "dictionary": {
///     "id": 1,
///     "name": "Common words"
///   },
///   "createdAtUTC": "2026-10-10T10:00:00Z"
/// }
/// </example>
public sealed record CreateEntryOutput(
    int Id,
    string Text,
    string Translation,
    DictionaryEntryType Type,
    string? PhoneticTranscription,
    DictionarySummaryOutput Dictionary,
    DateTime CreatedAtUTC
);

/// <summary>The requested entry.</summary>
/// <param name="Id">The entry's identifier.</param>
/// <param name="Text">The phrase or word text.</param>
/// <param name="Translation">The translation of the word or phrase.</param>
/// <param name="Type">
/// The type of entry.
/// 0 for Word type and 1 for Phrase type.
/// </param>
/// <param name="PhoneticTranscription">The phonetic transcription.</param>
/// <param name="Dictionary">The identifier and name of the dictionary the entry belongs to.</param>
/// <param name="CreatedAtUTC">The date and time when the entry was created, in UTC.</param>
/// <param name="LastUpdatedAtUTC">The date and time when the entry was last updated, in UTC.</param>
/// <example>
/// {
///   "id": 1,
///   "text": "Clanker",
///   "translation": "Battle droid",
///   "type": 0,
///   "phoneticTranscription": "ˈklæŋ.kər",
///   "dictionary": {
///     "id": 1,
///     "name": "Common words"
///   },
///   "createdAtUTC": "2026-10-10T10:00:00Z",
///   "lastUpdatedAtUTC": "2026-10-10T12:00:00Z"
/// }
/// </example>
public sealed record GetOneEntryOutput(
    int Id,
    string Text,
    string Translation,
    DictionaryEntryType Type,
    string? PhoneticTranscription,
    DictionarySummaryOutput Dictionary,
    DateTime CreatedAtUTC,
    DateTime LastUpdatedAtUTC
);

/// <summary>The updated entry.</summary>
/// <param name="Id">The entry's identifier.</param>
/// <param name="Text">The phrase or word text.</param>
/// <param name="Translation">The translation of the word or phrase.</param>
/// <param name="Type">
/// The type of entry.
/// 0 for Word type and 1 for Phrase type.
/// </param>
/// <param name="PhoneticTranscription">The phonetic transcription.</param>
/// <param name="Dictionary">The identifier and name of the dictionary the entry belongs to.</param>
/// <param name="LastUpdatedAtUTC">The date and time when the entry was last updated, in UTC.</param>
/// <example>
/// {
///   "id": 1,
///   "text": "Clanker",
///   "translation": "Battle droid",
///   "type": 0,
///   "phoneticTranscription": "ˈklæŋ.kər",
///   "dictionary": {
///     "id": 1,
///     "name": "Common words"
///   },
///   "lastUpdatedAtUTC": "2026-10-10T12:00:00Z"
/// }
/// </example>
public sealed record UpdateEntryOutput(
    int Id,
    string Text,
    string Translation,
    DictionaryEntryType Type,
    string? PhoneticTranscription,
    DictionarySummaryOutput Dictionary,
    DateTime LastUpdatedAtUTC
);

/// <summary>The requested page of entries.</summary>
/// <param name="Items">The entries on the requested page.</param>
/// <param name="Page">Page number.</param>
/// <param name="PageSize">Size of the page.</param>
/// <param name="TotalCount">Total number of results matching the supplied filters, before pagination.</param>
/// <example>
/// {
///   "items": [
///     {
///       "id": 1,
///       "text": "Clanker",
///       "translation": "Battle droid",
///       "type": 0,
///       "phoneticTranscription": "ˈklæŋ.kər",
///       "dictionary": {
///         "id": 1,
///         "name": "Common words"
///       },
///       "createdAtUTC": "2026-10-10T10:00:00Z",
///       "lastUpdatedAtUTC": "2026-10-10T12:00:00Z"
///     }
///   ],
///   "page": 1,
///   "pageSize": 20,
///   "totalCount": 1
/// }
/// </example>
public sealed record GetManyEntriesOutput(
    IReadOnlyList<GetOneEntryOutput> Items,
    int Page,
    int PageSize,
    int TotalCount
) : GetManyOutput(Page, PageSize, TotalCount);
