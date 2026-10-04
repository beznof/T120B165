namespace Languages.Application.Models.Outputs;

public record GetManyOutput(
    int Page,
    int PageSize,
    int TotalCount
);