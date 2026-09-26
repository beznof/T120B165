using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageLevel.Requests.Update;

public sealed class UpdateLanguageLevelRequest
{
    public PatchField<string?> Name { get; set; }
}
