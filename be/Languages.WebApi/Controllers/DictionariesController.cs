using System.Net;
using Languages.Application.Interfaces;
using Languages.Application.Models.Inputs.Common;
using Languages.Application.Models.Inputs.Dictionary;
using Languages.Application.Models.Outputs;
using Languages.WebApi.Extensions;
using Languages.WebApi.Models.Common;
using Languages.WebApi.Models.Requests.Dictionary;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Controllers;

[ApiController]
[Route("api/languages/{LanguageId:int}/dictionaries")]
public sealed class DictionariesController(IDictionaryService dictionaryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<GetManyDictionariesOutput?>>> GetMany([FromRoute] BaseDictionaryRequest dictionaryRequest, [FromQuery] GetManyDictionariesRequest request, CancellationToken cancellationToken)
    {
        var result = await dictionaryService.GetMany(new GetManyDictionariesInput
        {
            LanguageId = dictionaryRequest.LanguageId,
            Pagination = new GetManyInput
            {
                PageSize = request.PageSize,
                Page = request.Page,
                Search = request.Search,
            }
        }, cancellationToken);

        var links = result.IsSuccessful && result.Data is not null
            ? GetPaginationLinks(result.Data, dictionaryRequest, request)
            : null;

        return result.ToActionResult(links: links);
    }

    [HttpGet("{DictionaryId:int}")]
    public async Task<ActionResult<ApiResponse<GetOneDictionaryOutput?>>> GetOne([FromRoute] DictionaryRequest dictionaryRequest, CancellationToken cancellationToken)
    {
        var result = await dictionaryService.GetOne(new GetOneDictionaryInput
        {
            LanguageId = dictionaryRequest.LanguageId,
            DictionaryId = dictionaryRequest.DictionaryId
        }, cancellationToken);
        
        var links = result.IsSuccessful && result.Data is not null
            ? GetResourceLinks(dictionaryRequest.LanguageId, result.Data.Id)
            : null;

        return result.ToActionResult(links: links);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CreateDictionaryOutput>>> Create([FromRoute] BaseDictionaryRequest dictionaryRequest, [FromBody] CreateDictionaryRequest request, CancellationToken cancellationToken)
    {
        var result = await dictionaryService.Create(new CreateDictionaryInput
        {
            LanguageId = dictionaryRequest.LanguageId,
            Name = request.Name,
        }, cancellationToken);
        
        var links = result.IsSuccessful && result.Data is not null
            ? GetResourceLinks(dictionaryRequest.LanguageId, result.Data.Id)
            : null;

        return result.ToActionResult(statusCode: result.IsSuccessful ? HttpStatusCode.Created : null, links: links);
    }

    [HttpPatch("{DictionaryId:int}")]
    public async Task<ActionResult<ApiResponse>> Update([FromRoute] DictionaryRequest dictionaryRequest, [FromBody] UpdateDictionaryRequest request, CancellationToken cancellationToken)
    {
        var result = await dictionaryService.Update(new UpdateDictionaryInput
        {
            LanguageId = dictionaryRequest.LanguageId,
            DictionaryId = dictionaryRequest.DictionaryId,
            Name = request.Name,
        }, cancellationToken);
        
        var links = result.IsSuccessful ? GetResourceLinks(dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId) : null;

        return result.ToActionResult(links: links);
    }

    [HttpPut("{DictionaryId:int}/background-image")]
    public async Task<ActionResult<ApiResponse>> SetBackgroundImage([FromRoute] DictionaryRequest dictionaryRequest, [FromForm] SetDictionaryBackgroundImageRequest request, CancellationToken cancellationToken)
    {
        var result = await dictionaryService.SetBackgroundImage(new SetDictionaryBackgroundImageInput
        {
            LanguageId = dictionaryRequest.LanguageId,
            DictionaryId = dictionaryRequest.DictionaryId,
            Image = new SetImageInput
            {
                ContentType = request.BackgroundImage.ContentType,
                ImageStream = request.BackgroundImage.OpenReadStream(),
                Length = request.BackgroundImage.Length,
            }
        }, cancellationToken);
        
        var links = result.IsSuccessful ? GetResourceLinks(dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId) : null;

        return result.ToActionResult(links: links);
    }
    
    [HttpDelete("{DictionaryId:int}/background-image")]
    public async Task<ActionResult<ApiResponse>> DeleteBackgroundImage([FromRoute] DictionaryRequest dictionaryRequest, CancellationToken cancellationToken)
    {
        var result = await dictionaryService.DeleteBackgroundImage(new DeleteDictionaryBackgroundImageInput
        {
            LanguageId = dictionaryRequest.LanguageId,
            DictionaryId = dictionaryRequest.DictionaryId
        }, cancellationToken);
        
        var links = result.IsSuccessful ? GetResourceLinks(dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId) : null;

        return result.ToActionResult(links: links);
    }

    [HttpDelete("{DictionaryId:int}")]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] DictionaryRequest dictionaryRequest, CancellationToken cancellationToken)
    {
        var result = await dictionaryService.Delete(new DeleteDictionaryInput
        {
            LanguageId = dictionaryRequest.LanguageId,
            DictionaryId = dictionaryRequest.DictionaryId
        }, cancellationToken);
        
        var links = result.IsSuccessful ? GetCollectionLinks(dictionaryRequest.LanguageId) : null;

        return result.ToActionResult(links: links);
    }

    private Dictionary<string, string> GetCollectionLinks(int languageId) => new()
    {
        ["collection"] = Url.Action(nameof(GetMany), new { LanguageId = languageId, Page = 1, PageSize = 20 })!,
        ["language"] = Url.Action(nameof(LanguagesController.GetOne), "Languages", new { LanguageId = languageId })!
    };

    private Dictionary<string, string> GetResourceLinks(int languageId, int dictionaryId)
    {
        var links = GetCollectionLinks(languageId);
        links["self"] = Url.Action(nameof(GetOne), new { LanguageId = languageId, DictionaryId = dictionaryId })!;
        links["entries"] = Url.Action(nameof(EntriesController.GetMany), "Entries",
            new { LanguageId = languageId, DictionaryId = dictionaryId, Page = 1, PageSize = 20 })!;
        return links;
    }

    private Dictionary<string, string> GetPaginationLinks(GetManyOutput output, BaseDictionaryRequest scope, GetManyDictionariesRequest request)
    {
        string PageUrl(int page) => Url.Action(nameof(GetMany),
            new { scope.LanguageId, Page = page, output.PageSize, request.Search })!;

        var links = new Dictionary<string, string>
        {
            ["self"] = PageUrl(output.Page),
            ["language"] = Url.Action(nameof(LanguagesController.GetOne), "Languages", new { scope.LanguageId })!
        };

        if (output.Page > 1)
            links["prev"] = PageUrl(output.Page - 1);

        // Stay within the page limit enforced by GetManyInputValidator.
        if (output.Page < 10000 && (long)output.Page * output.PageSize < output.TotalCount)
            links["next"] = PageUrl(output.Page + 1);

        return links;
    }
}
