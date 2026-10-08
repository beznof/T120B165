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
public sealed partial class DictionariesController(IDictionaryService dictionaryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetMany([FromRoute] BaseDictionaryRequest dictionaryRequest, [FromQuery] GetManyDictionariesRequest request, CancellationToken cancellationToken)
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

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentLanguageLink(links, dictionaryRequest.LanguageId);
            AddPaginationLinks(links, dictionaryRequest.LanguageId, result.Data.TotalCount, request.Page, request.PageSize, request.Search);
        }

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
        
        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentLanguageLink(links, dictionaryRequest.LanguageId);
            AddSelfResourceLinks(links, dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId);
            AddSetBackgroundImageLink(links, dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId);
            if (!string.IsNullOrWhiteSpace(result.Data.BackgroundImageUrl))
                AddDeleteBackgroundImageLink(links, dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId);
        }

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
        
        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentLanguageLink(links, dictionaryRequest.LanguageId);
            AddSelfResourceLinks(links, dictionaryRequest.LanguageId, result.Data.Id);
            AddSetBackgroundImageLink(links, dictionaryRequest.LanguageId, result.Data.Id);
        }

        return result.ToActionResult(statusCode: result.IsSuccessful ? HttpStatusCode.Created : null, links: links);
    }

    [HttpPatch("{DictionaryId:int}")]
    public async Task<ActionResult<ApiResponse<UpdateDictionaryOutput>>> Update([FromRoute] DictionaryRequest dictionaryRequest, [FromBody] UpdateDictionaryRequest request, CancellationToken cancellationToken)
    {
        var result = await dictionaryService.Update(new UpdateDictionaryInput
        {
            LanguageId = dictionaryRequest.LanguageId,
            DictionaryId = dictionaryRequest.DictionaryId,
            Name = request.Name,
        }, cancellationToken);
        
        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentLanguageLink(links, dictionaryRequest.LanguageId);
            AddSelfResourceLinks(links, dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId);
            AddSetBackgroundImageLink(links, dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId);
            if (!string.IsNullOrWhiteSpace(result.Data.BackgroundImageUrl))
                AddDeleteBackgroundImageLink(links, dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId);
        }

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
        
        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddImageParentLink(links, dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId);
            AddDeleteBackgroundImageLink(links, dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId);
            AddSetBackgroundImageLink(links, dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId);
        }

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
        
        var links = new Dictionary<string, string>();
        if (result.IsSuccessful)
        {
            AddImageParentLink(links, dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId);
            AddSetBackgroundImageLink(links, dictionaryRequest.LanguageId, dictionaryRequest.DictionaryId);
        }

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
        
        var links = new Dictionary<string, string>();
        if (result.IsSuccessful)
        {
            AddParentLanguageLink(links, dictionaryRequest.LanguageId);
            AddParentLanguageCollectionLink(links, dictionaryRequest.LanguageId);
            AddCreateLink(links, dictionaryRequest.LanguageId);
        }

        return result.ToActionResult(links: links);
    }
}
