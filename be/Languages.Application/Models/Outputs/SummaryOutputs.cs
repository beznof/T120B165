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

/// <summary>The user's identifier and username.</summary>
/// <param name="Id">The user's identifier.</param>
/// <param name="Username">The username of the user.</param>
/// <example>
/// {
///   "id": 1,
///   "username": "Captain Rex"
/// }
/// </example>
public sealed record OwnerSummaryOutput(int Id, string Username);
