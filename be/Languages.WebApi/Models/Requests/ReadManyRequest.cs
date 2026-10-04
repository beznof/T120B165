namespace Languages.WebApi.Models.Requests;

public class ReadManyRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
}
