namespace Languages.WebApi.Models.Language.Responses;

public sealed record ReadLanguageResponse(
    int Id,
    string Name,
    string? BackgroundImageUrl
);
