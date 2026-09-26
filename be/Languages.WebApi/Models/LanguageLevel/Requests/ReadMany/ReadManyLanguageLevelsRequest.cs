using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageLevel.Requests.ReadMany;

public sealed class ReadManyLanguageLevelsRequest : ReadManyRequest
{
    public int LanguageID { get; set; }
}
