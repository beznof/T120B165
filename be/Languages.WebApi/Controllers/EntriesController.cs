using System.Net;
using Languages.Application.Interfaces;
using Languages.Application.Models.Inputs.Common;
using Languages.Application.Models.Inputs.Entry;
using Languages.Application.Models.Outputs;
using Languages.WebApi.Extensions;
using Languages.WebApi.Models.Common;
using Languages.WebApi.Models.Requests.Entry;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Controllers;

/// <summary>Entry management.</summary>
[ApiController]
[Route("api/languages/{LanguageId}/dictionaries/{DictionaryId}/entries")]
[Produces("application/json")]
public sealed partial class EntriesController(IEntryService entryService) : ControllerBase
{
    /// <summary>List entries.</summary>
    /// <remarks>
    /// List entries within the specified dictionary and language in paginated manner by supplying the page size, the page number, and optionally a text search filter and an entry type filter.
    /// </remarks>
    /// <returns>The requested page of entries list.</returns>
    /// <response code="200">The requested page of entries was retrieved successfully.</response>
    /// <response code="400">The path or query parameters were missing or incorrect.</response>
    /// <response code="404">The requested dictionary was not found in the specified language.</response>
    [HttpGet]
    [ProducesResponseType<ApiResponse<GetManyEntriesOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetManyEntriesOutput>>> GetMany([FromRoute] BaseEntryRequest entryRequest, [FromQuery] GetManyEntriesRequest request, CancellationToken cancellationToken)
    {
        var result = await entryService.GetMany(new GetManyEntriesInput
        {
            LanguageId = entryRequest.LanguageId,
            DictionaryId = entryRequest.DictionaryId,
            Pagination = new GetManyInput
            {
                PageSize = request.PageSize,
                Page = request.Page,
                Search = request.Search,
            },
            Type = request.Type,
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentDictionaryLink(links, entryRequest.LanguageId, entryRequest.DictionaryId);
            AddPaginationLinks(links, entryRequest.LanguageId, entryRequest.DictionaryId, result.Data.TotalCount, result.Data.Page, result.Data.PageSize, request.Search, request.Type);
        }

        return result.ToActionResult(links: links);
    }

    /// <summary>Get an entry by its identifier.</summary>
    /// <remarks>
    /// Get an entry by its identifier within the specified dictionary and language.
    /// </remarks>
    /// <response code="200">The entry was retrieved successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect.</response>
    /// <response code="404">The requested entry was not found in the specified dictionary and language.</response>
    [HttpGet("{EntryId}")]
    [ProducesResponseType<ApiResponse<GetOneEntryOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetOneEntryOutput>>> GetOne([FromRoute] EntryRequest entryRequest, CancellationToken cancellationToken)
    {
        var result = await entryService.GetOne(new GetOneEntryInput
        {
            LanguageId = entryRequest.LanguageId,
            DictionaryId = entryRequest.DictionaryId,
            EntryId = entryRequest.EntryId
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentDictionaryLink(links, entryRequest.LanguageId, entryRequest.DictionaryId);
            AddSelfResourceLinks(links, entryRequest.LanguageId, entryRequest.DictionaryId, result.Data.Id);
        }

        return result.ToActionResult(links: links);
    }

    /// <summary>Create an entry.</summary>
    /// <remarks>
    /// Create an entry within the specified dictionary and language by specifying its properties.
    /// </remarks>
    /// <response code="201">The entry was created successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect or the JSON body was malformed or incorrect.</response>
    /// <response code="404">The requested dictionary was not found in the specified language.</response>
    [HttpPost]
    [ProducesResponseType<ApiResponse<CreateEntryOutput>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CreateEntryOutput>>> Create([FromRoute] BaseEntryRequest entryRequest, [FromBody] CreateEntryRequest request, CancellationToken cancellationToken)
    {
        var result = await entryService.Create(new CreateEntryInput
        {
            LanguageId = entryRequest.LanguageId,
            DictionaryId = entryRequest.DictionaryId,
            Text = request.Text,
            Translation = request.Translation,
            Type = request.Type,
            PhoneticTranscription = request.PhoneticTranscription,
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentDictionaryLink(links, entryRequest.LanguageId, entryRequest.DictionaryId);
            AddSelfResourceLinks(links, entryRequest.LanguageId, entryRequest.DictionaryId, result.Data.Id);
        }

        return result.ToActionResult(statusCode: result.IsSuccessful ? HttpStatusCode.Created : null, links: links);
    }

    /// <summary>Update an entry.</summary>
    /// <remarks>
    /// Modify an entry, indicated by its identifier, within the specified dictionary and language by specifying its new property values.
    /// </remarks>
    /// <response code="200">The entry was updated successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect or the JSON body was malformed or incorrect.</response>
    /// <response code="404">The requested entry was not found in the specified dictionary and language.</response>
    [HttpPatch("{EntryId}")]
    [ProducesResponseType<ApiResponse<UpdateEntryOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UpdateEntryOutput>>> Update([FromRoute] EntryRequest entryRequest, [FromBody] UpdateEntryRequest request, CancellationToken cancellationToken)
    {
        var result = await entryService.Update(new UpdateEntryInput
        {
            LanguageId = entryRequest.LanguageId,
            DictionaryId = entryRequest.DictionaryId,
            EntryId = entryRequest.EntryId,
            Text = request.Text,
            Translation = request.Translation,
            Type = request.Type,
            PhoneticTranscription = request.PhoneticTranscription,
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentDictionaryLink(links, entryRequest.LanguageId, entryRequest.DictionaryId);
            AddSelfResourceLinks(links, entryRequest.LanguageId, entryRequest.DictionaryId, entryRequest.EntryId);
        }

        return result.ToActionResult(links: links);
    }

    /// <summary>Delete an entry.</summary>
    /// <remarks>
    /// Delete an entry, indicated by its identifier, within the specified dictionary and language.
    /// </remarks>
    /// <response code="200">The entry was deleted successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect.</response>
    /// <response code="404">The requested entry was not found in the specified dictionary and language.</response>
    [HttpDelete("{EntryId}")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] EntryRequest entryRequest, CancellationToken cancellationToken)
    {
        var result = await entryService.Delete(new DeleteEntryInput
        {
            LanguageId = entryRequest.LanguageId,
            DictionaryId = entryRequest.DictionaryId,
            EntryId = entryRequest.EntryId
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful)
        {
            AddParentDictionaryLink(links, entryRequest.LanguageId, entryRequest.DictionaryId);
            AddParentDictionaryCollectionLink(links, entryRequest.LanguageId, entryRequest.DictionaryId);
            AddCreateLink(links, entryRequest.LanguageId, entryRequest.DictionaryId);
        }

        return result.ToActionResult(links: links);
    }

}
