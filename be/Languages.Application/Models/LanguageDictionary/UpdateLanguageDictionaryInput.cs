using Languages.Application.Models.Common;

namespace Languages.Application.Models.LanguageDictionary;

public sealed class UpdateLanguageDictionaryInput
{
    public Optional<string?> Name { get; init; }
}
