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

[ApiController]
[Route("api/languages/{languageId}/dictionaries/{dictionaryId}/entries")]
public sealed partial class EntriesController(IEntryService entryService) : ControllerBase
{
    [HttpGet]
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

    [HttpGet("{EntryId}")]
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

    [HttpPost]
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

    [HttpPatch("{EntryId}")]
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

    [HttpDelete("{EntryId}")]
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
