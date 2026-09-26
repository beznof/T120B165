using Languages.Domain.Enums;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.DictionaryEntry.Requests.Update;

public sealed class UpdateDictionaryEntryRequest
{
    public PatchField<string?> Text { get; set; }
    public PatchField<string?> Translation { get; set; }
    public PatchField<DictionaryEntryType?> Type { get; set; }
    public PatchField<string?> PhoneticTranscription { get; set; }
    public PatchField<List<string>?> Synonyms { get; set; }
}
