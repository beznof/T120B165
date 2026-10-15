using System.Net;
using System.Security.Claims;
using Languages.Application.Interfaces;
using Languages.Application.Models.Inputs.Common;
using Languages.Application.Models.Inputs.Entry;
using Languages.Application.Models.Outputs;
using Languages.Domain.Enums;
using Languages.WebApi.Extensions;
using Languages.WebApi.Models.Common;
using Languages.WebApi.Models.Requests.Entry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Controllers;

/// <summary>Entry management.</summary>
[ApiController]
[Route("api/languages/{LanguageId}/dictionaries/{DictionaryId}/entries")]
[Produces("application/json")]
[Authorize(Roles = $"{nameof(UserRole.Learner)},{nameof(UserRole.Teacher)},{nameof(UserRole.Admin)}")]
public sealed partial class EntriesController(IEntryService entryService) : ControllerBase
{
    private int UserId => int.TryParse(User.FindFirstValue("sub"), out var userId) 
        ? userId 
        : throw new InvalidDataException("Couldn't retrieve the user's ID");
    private bool IsAdmin => User.IsInRole(nameof(UserRole.Admin));
    
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
        var languageId = entryRequest.LanguageId!.Value;
        var dictionaryId = entryRequest.DictionaryId!.Value;
        var pageSize = request.PageSize!.Value;
        var page = request.Page!.Value;
        var search = request.Search;
        var type = request.Type;

        var result = await entryService.GetMany(new GetManyEntriesInput
        {
            LanguageId = languageId,
            DictionaryId = dictionaryId,
            Pagination = new GetManyInput
            {
                PageSize = pageSize,
                Page = page,
                Search = search,
            },
            Type = type,
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentDictionaryLink(links, languageId, dictionaryId);
            AddPaginationLinks(links, languageId, dictionaryId, result.Data.TotalCount, result.Data.Page, result.Data.PageSize, search, type);
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
        var languageId = entryRequest.LanguageId!.Value;
        var dictionaryId = entryRequest.DictionaryId!.Value;
        var entryId = entryRequest.EntryId!.Value;

        var result = await entryService.GetOne(new GetOneEntryInput
        {
            LanguageId = languageId,
            DictionaryId = dictionaryId,
            EntryId = entryId
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentDictionaryLink(links, languageId, dictionaryId);
            AddSelfResourceLinks(links, languageId, dictionaryId, result.Data.Id);
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
    [Authorize(Roles = $"{nameof(UserRole.Teacher)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType<ApiResponse<CreateEntryOutput>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CreateEntryOutput>>> Create([FromRoute] BaseEntryRequest entryRequest, [FromBody] CreateEntryRequest request, CancellationToken cancellationToken)
    {
        var languageId = entryRequest.LanguageId!.Value;
        var dictionaryId = entryRequest.DictionaryId!.Value;
        var text = request.Text;
        var translation = request.Translation;
        var type = request.Type!.Value;
        var phoneticTranscription = request.PhoneticTranscription;

        var result = await entryService.Create(new CreateEntryInput
            {
            LanguageId = languageId,
            DictionaryId = dictionaryId,
            Text = text,
            Translation = translation,
            Type = type,
            PhoneticTranscription = phoneticTranscription
            }, 
            new AuthorInput
            {
                AuthorId = UserId,
                IsAdmin = IsAdmin
            }
            , cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentDictionaryLink(links, languageId, dictionaryId);
            AddSelfResourceLinks(links, languageId, dictionaryId, result.Data.Id);
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
    [Authorize(Roles = $"{nameof(UserRole.Teacher)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType<ApiResponse<UpdateEntryOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UpdateEntryOutput>>> Update([FromRoute] EntryRequest entryRequest, [FromBody] UpdateEntryRequest request, CancellationToken cancellationToken)
    {
        var languageId = entryRequest.LanguageId!.Value;
        var dictionaryId = entryRequest.DictionaryId!.Value;
        var entryId = entryRequest.EntryId!.Value;
        var text = request.Text;
        var translation = request.Translation;
        var type = request.Type;
        var phoneticTranscription = request.PhoneticTranscription;

        var result = await entryService.Update(new UpdateEntryInput
            {
            LanguageId = languageId,
            DictionaryId = dictionaryId,
            EntryId = entryId,
            Text = text,
            Translation = translation,
            Type = type,
            PhoneticTranscription = phoneticTranscription
            }, 
            new AuthorInput
            {
                AuthorId = UserId,
                IsAdmin = IsAdmin
            }
            , cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentDictionaryLink(links, languageId, dictionaryId);
            AddSelfResourceLinks(links, languageId, dictionaryId, entryId);
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
    [Authorize(Roles = $"{nameof(UserRole.Teacher)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] EntryRequest entryRequest, CancellationToken cancellationToken)
    {
        var languageId = entryRequest.LanguageId!.Value;
        var dictionaryId = entryRequest.DictionaryId!.Value;
        var entryId = entryRequest.EntryId!.Value;

        var result = await entryService.Delete(new DeleteEntryInput
            {
                LanguageId = languageId,
                DictionaryId = dictionaryId,
                EntryId = entryId
            }, 
            new AuthorInput
            {
                AuthorId = UserId,
                IsAdmin = IsAdmin
            }
            , cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful)
        {
            AddParentDictionaryLink(links, languageId, dictionaryId);
            AddParentDictionaryCollectionLink(links, languageId, dictionaryId);
            AddCreateLink(links, languageId, dictionaryId);
        }

        return result.ToActionResult(links: links);
    }

}
