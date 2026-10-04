namespace Languages.WebApi.Models.Requests.Entry;

public sealed class EntryRequest : BaseEntryRequest
{
    public required int EntryId { get; set; }
}
