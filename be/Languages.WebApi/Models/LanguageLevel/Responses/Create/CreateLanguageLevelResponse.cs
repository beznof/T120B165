namespace Languages.WebApi.Models.LanguageLevel.Responses.Create;

public sealed record CreateLanguageLevelResponse(
    int Id,
    string Name,
    int LanguageID
);
