using Languages.Application.Models.Common;
using Languages.Domain.Enums;

namespace Languages.WebApi.Models.Requests.Entry;

public sealed class UpdateEntryRequest
{
    public Optional<string?> Text { get; set; }
    public Optional<string?> Translation { get; set; }
    public Optional<DictionaryEntryType?> Type { get; set; }
    public Optional<string?> PhoneticTranscription { get; set; }
}
