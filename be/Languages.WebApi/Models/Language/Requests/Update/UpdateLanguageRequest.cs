using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Language.Requests.Update;

public sealed class UpdateLanguageRequest
{
    public PatchField<string?> Name { get; set; }
}
