namespace Languages.Application.Models.Outputs;

/// <summary>The language's identifier and name.</summary>
/// <param name="Id">The language's identifier.</param>
/// <param name="Name">The name of the language.</param>
/// <example>
/// {
///   "id": 1,
///   "name": "Ewokese"
/// }
/// </example>
public sealed record LanguageSummaryOutput(int Id, string Name);

/// <summary>The dictionary's identifier and name.</summary>
/// <param name="Id">The dictionary's identifier.</param>
/// <param name="Name">The name of the dictionary.</param>
/// <example>
/// {
///   "id": 1,
///   "name": "Common words"
/// }
/// </example>
public sealed record DictionarySummaryOutput(int Id, string Name);
