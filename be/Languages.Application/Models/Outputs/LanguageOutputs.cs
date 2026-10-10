namespace Languages.Application.Models.Outputs;

/// <summary>The created language.</summary>
/// <param name="Id">The language's identifier.</param>
/// <param name="Name">The name of the language.</param>
/// <param name="BackgroundImageUrl">
/// The URL of the language's background image.
/// </param>
/// <param name="CreatedAtUTC">The date and time when the language was created, in UTC.</param>
/// <example>
/// {
///   "id": 1,
///   "name": "Ewokese",
///   "backgroundImageUrl": null,
///   "createdAtUTC": "2026-10-10T10:00:00Z"
/// }
/// </example>
public sealed record CreateLanguageOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    DateTime CreatedAtUTC
);

/// <summary>The requested language.</summary>
/// <param name="Id">The language's identifier.</param>
/// <param name="Name">The name of the language.</param>
/// <param name="BackgroundImageUrl">
/// The URL of the language's background image.
/// </param>
/// <param name="CreatedAtUTC">The date and time when the language was created, in UTC.</param>
/// <param name="LastUpdatedAtUTC">The date and time when the language was last updated, in UTC.</param>
/// <example>
/// {
///   "id": 1,
///   "name": "Ewokese",
///   "backgroundImageUrl": null,
///   "createdAtUTC": "2026-10-10T10:00:00Z",
///   "lastUpdatedAtUTC": "2026-10-10T12:00:00Z"
/// }
/// </example>
public sealed record GetOneLanguageOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    DateTime CreatedAtUTC,
    DateTime LastUpdatedAtUTC
);

/// <summary>The updated language.</summary>
/// <param name="Id">The language's identifier.</param>
/// <param name="Name">The name of the language.</param>
/// <param name="BackgroundImageUrl">
/// The URL of the language's background image.
/// </param>
/// <param name="LastUpdatedAtUTC">The date and time when the language was last updated, in UTC.</param>
/// <example>
/// {
///   "id": 1,
///   "name": "Ewokese",
///   "backgroundImageUrl": null,
///   "lastUpdatedAtUTC": "2026-10-10T12:00:00Z"
/// }
/// </example>
public sealed record UpdateLanguageOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    DateTime LastUpdatedAtUTC
);

/// <summary>The requested page of languages.</summary>
/// <param name="Items">The languages on the requested page.</param>
/// <param name="Page">Page number. Starts at 1.</param>
/// <param name="PageSize">Size of the page.</param>
/// <param name="TotalCount">Total number of results matching the supplied filters, before pagination.</param>
/// <example>
/// {
///   "items": [
///     {
///       "id": 1,
///       "name": "Ewokese",
///       "backgroundImageUrl": null,
///       "createdAtUTC": "2026-10-10T10:00:00Z",
///       "lastUpdatedAtUTC": "2026-10-10T12:00:00Z"
///     }
///   ],
///   "page": 1,
///   "pageSize": 20,
///   "totalCount": 1
/// }
/// </example>
public sealed record GetManyLanguagesOutput(
    IReadOnlyList<GetOneLanguageOutput> Items,
    int Page,
    int PageSize,
    int TotalCount
) : GetManyOutput(Page, PageSize, TotalCount);

/// <summary>The uploaded background image.</summary>
/// <param name="ImageUrl">The URL of the language's uploaded background image.</param>
/// <example>
/// {
///   "imageUrl": "https://example.com/images/languages/1.png"
/// }
/// </example>
public sealed record SetLanguageBackgroundImageOutput(
    string ImageUrl
);
