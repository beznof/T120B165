using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageDictionary.Requests.ReadMany;

public sealed class ReadManyLanguageDictionariesRequest : ReadManyRequest
{
    public int LanguageID { get; set; }
    public int[] LevelIDs { get; set; } = [];
}
