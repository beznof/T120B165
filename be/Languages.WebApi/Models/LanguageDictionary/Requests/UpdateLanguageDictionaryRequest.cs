using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageDictionary.Requests;

public sealed class UpdateLanguageDictionaryRequest
{
    public PatchField<string?> Name { get; set; }
}
