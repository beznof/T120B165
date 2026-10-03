namespace Languages.WebApi.Models.Language.Responses;

public sealed record CreateLanguageResponse(
    int Id,
    string Name,
    string? BackgroundImageUrl
);
