namespace Languages.WebApi.Models.Common;

public sealed record ApiResponse(
    bool Success,
    string Message,
    string? UserFriendlyMessage,
    object? Data = null,
    ICollection<string>? Errors = null
);
