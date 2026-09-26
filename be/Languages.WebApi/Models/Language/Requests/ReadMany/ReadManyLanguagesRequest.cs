using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Language.Requests.ReadMany;

public sealed class ReadManyLanguagesRequest : ReadManyRequest
{
    public int[] LanguageFamilyIDs { get; set; } = [];
}
