namespace Languages.WebApi.Models.Responses.Language;

public sealed record ReadLanguageResponse(
    int Id,
    string Name,
    string? BackgroundImageUrl
);
