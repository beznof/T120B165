namespace Languages.Application.Models.Outputs;

/// <summary>The created dictionary.</summary>
/// <param name="Id">The dictionary's identifier.</param>
/// <param name="Name">The name of the dictionary.</param>
/// <param name="BackgroundImageUrl">
/// The URL of the dictionary's background image.
/// </param>
/// <param name="Language">The identifier and name of the language the dictionary belongs to.</param>
/// <param name="CreatedAtUTC">The date and time when the dictionary was created, in UTC.</param>
/// <example>
/// {
///   "id": 1,
///   "name": "Common words",
///   "backgroundImageUrl": null,
///   "language": {
///     "id": 1,
///     "name": "Ewokese"
///   },
///   "createdAtUTC": "2026-10-10T10:00:00Z"
/// }
/// </example>
public sealed record CreateDictionaryOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    LanguageSummaryOutput Language,
    DateTime CreatedAtUTC
);

/// <summary>The requested dictionary.</summary>
/// <param name="Id">The dictionary's identifier.</param>
/// <param name="Name">The name of the dictionary.</param>
/// <param name="BackgroundImageUrl">
/// The URL of the dictionary's background image.
/// </param>
/// <param name="Language">The identifier and name of the language the dictionary belongs to.</param>
/// <param name="CreatedAtUTC">The date and time when the dictionary was created, in UTC.</param>
/// <param name="LastUpdatedAtUTC">The date and time when the dictionary was last updated, in UTC.</param>
/// <example>
/// {
///   "id": 1,
///   "name": "Common words",
///   "backgroundImageUrl": null,
///   "language": {
///     "id": 1,
///     "name": "Ewokese"
///   },
///   "createdAtUTC": "2026-10-10T10:00:00Z",
///   "lastUpdatedAtUTC": "2026-10-10T12:00:00Z"
/// }
/// </example>
public sealed record GetOneDictionaryOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    LanguageSummaryOutput Language,
    DateTime CreatedAtUTC,
    DateTime LastUpdatedAtUTC
);

/// <summary>The updated dictionary.</summary>
/// <param name="Id">The dictionary's identifier.</param>
/// <param name="Name">The name of the dictionary.</param>
/// <param name="BackgroundImageUrl">
/// The URL of the dictionary's background image.
/// </param>
/// <param name="Language">The identifier and name of the language the dictionary belongs to.</param>
/// <param name="LastUpdatedAtUTC">The date and time when the dictionary was last updated, in UTC.</param>
/// <example>
/// {
///   "id": 1,
///   "name": "Common words",
///   "backgroundImageUrl": null,
///   "language": {
///     "id": 1,
///     "name": "Ewokese"
///   },
///   "lastUpdatedAtUTC": "2026-10-10T12:00:00Z"
/// }
/// </example>
public sealed record UpdateDictionaryOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    LanguageSummaryOutput Language,
    DateTime LastUpdatedAtUTC
);

/// <summary>The requested page of dictionaries.</summary>
/// <param name="Items">The dictionaries on the requested page.</param>
/// <param name="Page">Page number</param>
/// <param name="PageSize">Size of the page.</param>
/// <param name="TotalCount">Total number of results matching the supplied filters, before pagination.</param>
/// <example>
/// {
///   "items": [
///     {
///       "id": 1,
///       "name": "Common words",
///       "backgroundImageUrl": null,
///       "language": {
///         "id": 1,
///         "name": "Ewokese"
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
public sealed record GetManyDictionariesOutput(
    IReadOnlyList<GetOneDictionaryOutput> Items,
    int Page,
    int PageSize,
    int TotalCount
) : GetManyOutput(Page, PageSize, TotalCount);

/// <summary>The uploaded background image.</summary>
/// <param name="ImageUrl">The URL of the dictionary's uploaded background image.</param>
/// <example>
/// {
///   "imageUrl": "https://example.com/images/dictionaries/1.png"
/// }
/// </example>
public sealed record SetDictionaryBackgroundImageOutput(
    string ImageUrl
);
