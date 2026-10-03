namespace Languages.Application.Models.Common;

public class ReadManyInput
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public string? Search { get; init; }
}
