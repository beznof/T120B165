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
[Route("api/languages/{languageId:int}/dictionaries/{dictionaryId:int}/entries")]
public sealed class EntriesController(IEntryService entryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<GetManyEntriesOutput?>>> GetMany([FromRoute] BaseEntryRequest entryRequest, [FromQuery] GetManyEntriesRequest request, CancellationToken cancellationToken)
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

        var links = result.IsSuccessful && result.Data is not null
            ? GetPaginationLinks(result.Data, entryRequest, request)
            : null;

        return result.ToActionResult(links: links);
    }

    [HttpGet("{EntryId:int}")]
    public async Task<ActionResult<ApiResponse<GetOneEntryOutput?>>> GetOne([FromRoute] EntryRequest entryRequest, CancellationToken cancellationToken)
    {
        var result = await entryService.GetOne(new GetOneEntryInput
        {
            LanguageId = entryRequest.LanguageId,
            DictionaryId = entryRequest.DictionaryId,
            EntryId = entryRequest.EntryId
        }, cancellationToken);

        var links = result.IsSuccessful && result.Data is not null
            ? GetResourceLinks(entryRequest.LanguageId, entryRequest.DictionaryId, result.Data.Id)
            : null;

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

        var links = result.IsSuccessful && result.Data is not null
            ? GetResourceLinks(entryRequest.LanguageId, entryRequest.DictionaryId, result.Data.Id)
            : null;

        return result.ToActionResult(statusCode: result.IsSuccessful ? HttpStatusCode.Created : null, links: links);
    }

    [HttpPatch("{EntryId:int}")]
    public async Task<ActionResult<ApiResponse>> Update([FromRoute] EntryRequest entryRequest, [FromBody] UpdateEntryRequest request, CancellationToken cancellationToken)
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

        var links = result.IsSuccessful ? GetResourceLinks(entryRequest.LanguageId, entryRequest.DictionaryId, entryRequest.EntryId) : null;

        return result.ToActionResult(links: links);
    }

    [HttpDelete("{EntryId:int}")]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] EntryRequest entryRequest, CancellationToken cancellationToken)
    {
        var result = await entryService.Delete(new DeleteEntryInput
        {
            LanguageId = entryRequest.LanguageId,
            DictionaryId = entryRequest.DictionaryId,
            EntryId = entryRequest.EntryId
        }, cancellationToken);

        var links = result.IsSuccessful ? GetCollectionLinks(entryRequest.LanguageId, entryRequest.DictionaryId) : null;

        return result.ToActionResult(links: links);
    }

    private Dictionary<string, string> GetCollectionLinks(int languageId, int dictionaryId) => new()
    {
        ["collection"] = Url.Action(nameof(GetMany),
            new { LanguageId = languageId, DictionaryId = dictionaryId, Page = 1, PageSize = 20 })!,
        ["dictionary"] = Url.Action(nameof(DictionariesController.GetOne), "Dictionaries",
            new { LanguageId = languageId, DictionaryId = dictionaryId })!
    };

    private Dictionary<string, string> GetResourceLinks(int languageId, int dictionaryId, int entryId)
    {
        var links = GetCollectionLinks(languageId, dictionaryId);
        links["self"] = Url.Action(nameof(GetOne),
            new { LanguageId = languageId, DictionaryId = dictionaryId, EntryId = entryId })!;
        return links;
    }

    private Dictionary<string, string> GetPaginationLinks(GetManyOutput output, BaseEntryRequest scope, GetManyEntriesRequest request)
    {
        string PageUrl(int page) => Url.Action(nameof(GetMany),
            new { scope.LanguageId, scope.DictionaryId, Page = page, output.PageSize, request.Search, request.Type })!;

        var links = new Dictionary<string, string>
        {
            ["self"] = PageUrl(output.Page),
            ["dictionary"] = Url.Action(nameof(DictionariesController.GetOne), "Dictionaries",
                new { scope.LanguageId, scope.DictionaryId })!
        };

        if (output.Page > 1)
            links["prev"] = PageUrl(output.Page - 1);

        // Stay within the page limit enforced by GetManyInputValidator.
        if (output.Page < 10000 && (long)output.Page * output.PageSize < output.TotalCount)
            links["next"] = PageUrl(output.Page + 1);

        return links;
    }
}
