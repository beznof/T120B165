namespace Languages.WebApi.Models.Common;

public sealed record LanguageSummaryResponse(int Id, string Name);
public sealed record LanguageLevelSummaryResponse(int Id, string Name);
public sealed record LanguageFamilySummaryResponse(int Id, string Name);
public sealed record LanguageDictionarySummaryResponse(int Id, string Name);
