using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Language.Requests;

public sealed class UpdateLanguageRequest
{
    public PatchField<string?> Name { get; set; }
}
