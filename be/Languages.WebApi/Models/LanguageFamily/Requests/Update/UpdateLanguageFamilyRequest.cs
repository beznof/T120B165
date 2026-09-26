using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageFamily.Requests.Update;

public sealed class UpdateLanguageFamilyRequest
{
    public PatchField<string?> Name { get; set; }
}
