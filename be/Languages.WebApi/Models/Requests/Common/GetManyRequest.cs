namespace Languages.WebApi.Models.Requests.Common;

public class GetManyRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
}
