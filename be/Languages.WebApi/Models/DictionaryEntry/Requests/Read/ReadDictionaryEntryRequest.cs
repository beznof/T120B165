namespace Languages.WebApi.Models.DictionaryEntry.Requests.Read;

public sealed class ReadDictionaryEntryRequest
{
    public required int Id { get; set; }
    public required int DictionaryID { get; set; }
}
