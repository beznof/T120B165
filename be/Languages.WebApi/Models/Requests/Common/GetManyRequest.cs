using Languages.WebApi.OpenApi;

namespace Languages.WebApi.Models.Requests.Common;

public class GetManyRequest
{
    /// <summary>
    /// Page number.
    /// </summary>
    /// <example>1</example>
    [OpenApiRequired]
    public int Page { get; set; }
    /// <summary>
    /// Size of the page.
    /// </summary>
    /// <example>20</example>
    [OpenApiRequired]
    public int PageSize { get; set; }
    /// <summary>
    /// Optional search term for filtering the results.
    /// </summary>
    /// <example>abc</example>
    public string? Search { get; set; }
}
