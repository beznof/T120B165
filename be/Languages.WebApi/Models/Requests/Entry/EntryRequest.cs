using System.ComponentModel.DataAnnotations;

namespace Languages.WebApi.Models.Requests.Entry;

public sealed class EntryRequest : BaseEntryRequest
{
    /// <summary>
    /// The entry's identifier.
    /// Must be a positive integer.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Entry ID is required.")]
    public int? EntryId { get; set; }
}
