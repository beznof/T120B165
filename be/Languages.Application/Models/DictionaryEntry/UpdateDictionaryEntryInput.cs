using Languages.Application.Models.Common;
using Languages.Domain.Enums;

namespace Languages.Application.Models.DictionaryEntry;

public sealed class UpdateDictionaryEntryInput
{
    public Optional<string?> Text { get; init; }
    public Optional<string?> Translation { get; init; }
    public Optional<DictionaryEntryType?> Type { get; init; }
    public Optional<string?> PhoneticTranscription { get; init; }
}
