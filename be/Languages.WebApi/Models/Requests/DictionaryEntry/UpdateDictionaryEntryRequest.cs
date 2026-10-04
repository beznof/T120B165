using Languages.Application.Models.Common;
using Languages.Domain.Enums;

namespace Languages.WebApi.Models.Requests.DictionaryEntry;

public sealed class UpdateDictionaryEntryRequest
{
    public Optional<string?> Text { get; set; }
    public Optional<string?> Translation { get; set; }
    public Optional<DictionaryEntryType?> Type { get; set; }
    public Optional<string?> PhoneticTranscription { get; set; }
}
