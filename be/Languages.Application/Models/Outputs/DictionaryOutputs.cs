namespace Languages.Application.Models.Outputs;

public sealed record CreateDictionaryOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    LanguageSummaryOutput Language
);

public sealed record GetOneDictionaryOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    LanguageSummaryOutput Language
);

public sealed record UpdateDictionaryOutput(
    int Id,
    string Name,
    string? BackgroundImageUrl,
    LanguageSummaryOutput Language
);

public sealed record GetManyDictionariesOutput(
    IReadOnlyList<GetOneDictionaryOutput> Items,
    int Page,
    int PageSize,
    int TotalCount
) : GetManyOutput(Page, PageSize, TotalCount);

public sealed record SetDictionaryBackgroundImageOutput(
    string ImageUrl
);
