namespace Languages.WebApi.Models.Responses.Language;

public sealed record CreateLanguageResponse(
    int Id,
    string Name,
    string? BackgroundImageUrl
);
