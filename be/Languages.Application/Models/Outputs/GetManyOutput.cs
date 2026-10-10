namespace Languages.Application.Models.Outputs;

/// <summary>The pagination details of the requested page.</summary>
/// <param name="Page">Page number.</param>
/// <param name="PageSize">Size of the page.</param>
/// <param name="TotalCount">Total number of results matching the supplied filters, before pagination.</param>
/// <example>
/// {
///   "page": 1,
///   "pageSize": 20,
///   "totalCount": 1
/// }
/// </example>
public record GetManyOutput(
    int Page,
    int PageSize,
    int TotalCount
);
