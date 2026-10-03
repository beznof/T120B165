using Languages.Application.Models.Common;
using Languages.Domain.Enums;

namespace Languages.Application.Models.DictionaryEntry;

public sealed class ReadManyDictionaryEntriesInput : ReadManyInput
{
    public DictionaryEntryType? Type { get; init; }
}
