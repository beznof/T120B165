namespace Languages.WebApi.Models.LanguageLevel.Responses.Read;

public sealed record ReadLanguageLevelResponse(
    int Id,
    string Name,
    int LanguageID
);
