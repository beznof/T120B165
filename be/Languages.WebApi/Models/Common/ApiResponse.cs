namespace Languages.WebApi.Models.Common;

public sealed record ApiResponse(
    string Message,
    object? Data = null,
    ICollection<string>? Errors = null
);
