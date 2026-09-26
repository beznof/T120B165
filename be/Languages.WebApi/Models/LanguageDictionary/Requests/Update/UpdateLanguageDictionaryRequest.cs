using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageDictionary.Requests.Update;

public sealed class UpdateLanguageDictionaryRequest
{
    public PatchField<string?> Name { get; set; }
    public PatchField<int?> LevelID { get; set; }
}
