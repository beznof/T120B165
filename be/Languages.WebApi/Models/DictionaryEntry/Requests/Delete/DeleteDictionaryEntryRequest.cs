namespace Languages.WebApi.Models.DictionaryEntry.Requests.Delete;

public sealed class DeleteDictionaryEntryRequest
{
    public required int Id { get; set; }
    public required int DictionaryID { get; set; }
}
