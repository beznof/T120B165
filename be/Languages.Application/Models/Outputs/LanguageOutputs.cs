namespace Languages.Application.Models.Outputs;

public sealed record CreateLanguageOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl
);

public sealed record GetOneLanguageOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl
);

public sealed record UpdateLanguageOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl
);

public sealed record GetManyLanguagesOutput(
    IReadOnlyList<GetOneLanguageOutput> Items,
    int Page,
    int PageSize,
    int TotalCount
) : GetManyOutput(Page, PageSize, TotalCount);

public sealed record SetLanguageBackgroundImageOutput(
    string ImageUrl
);
