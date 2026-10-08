using System.Net;
using Languages.Application.Interfaces;
using Languages.Application.Models.Inputs.Common;
using Languages.Application.Models.Inputs.Language;
using Languages.Application.Models.Outputs;
using Languages.WebApi.Extensions;
using Languages.WebApi.Models.Common;
using Languages.WebApi.Models.Requests.Language;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Controllers;

[ApiController]
[Route("api/languages")]
public sealed partial class LanguagesController(ILanguageService languageService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<GetManyLanguagesOutput>>> GetMany([FromQuery] GetManyLanguagesRequest request, CancellationToken cancellationToken)
    {
        var result = await languageService.GetMany(new GetManyLanguagesInput
        {
            Pagination = new GetManyInput
            {
                PageSize = request.PageSize,
                Page = request.Page,
                Search = request.Search,
            }
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
            AddPaginationLinks(links, result.Data.TotalCount, result.Data.Page, result.Data.PageSize, request.Search);

        return result.ToActionResult(links: links);
    }

    [HttpGet("{LanguageId:int}")]
    public async Task<ActionResult<ApiResponse<GetOneLanguageOutput>>> GetOne([FromRoute] LanguageRequest languageRequest, CancellationToken cancellationToken)
    {
        var result = await languageService.GetOne(new GetOneLanguageInput
        {
            LanguageId = languageRequest.LanguageId
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddSelfResourceLinks(links, result.Data.Id);
            AddSetBackgroundImageLink(links, result.Data.Id);
            if (!string.IsNullOrWhiteSpace(result.Data.BackgroundImageUrl))
                AddDeleteBackgroundImageLink(links, result.Data.Id);
        }

        return result.ToActionResult(links: links);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CreateLanguageOutput>>> Create([FromForm] CreateLanguageRequest request, CancellationToken cancellationToken)
    {
        var result = await languageService.Create(new CreateLanguageInput
        {
            Name = request.Name,
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data is not null)
        {
            AddSelfResourceLinks(links, result.Data.Id);
            AddSetBackgroundImageLink(links, result.Data.Id);
        }

        return result.ToActionResult(statusCode: result.IsSuccessful ? HttpStatusCode.Created : null, links: links);
    }

    [HttpPatch("{LanguageId:int}")]
    public async Task<ActionResult<ApiResponse<UpdateLanguageOutput>>> Update([FromRoute] LanguageRequest languageRequest, [FromBody] UpdateLanguageRequest request, CancellationToken cancellationToken)
    {
        var result = await languageService.Update(new UpdateLanguageInput
        {
            LanguageId = languageRequest.LanguageId,
            Name = request.Name,
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddSelfResourceLinks(links, languageRequest.LanguageId);
            AddSetBackgroundImageLink(links, languageRequest.LanguageId);
            if (!string.IsNullOrWhiteSpace(result.Data.BackgroundImageUrl))
                AddDeleteBackgroundImageLink(links, languageRequest.LanguageId);
        }

        return result.ToActionResult(links: links);
    }

    [HttpPut("{LanguageId:int}/background-image")]
    public async Task<ActionResult<ApiResponse<SetLanguageBackgroundImageOutput>>> SetBackgroundImage([FromRoute] LanguageRequest languageRequest, [FromForm] SetLanguageBackgroundImageRequest request, CancellationToken cancellationToken)
    {
        var result = await languageService.SetBackgroundImage(new SetLanguageBackgroundImageInput
        {
            LanguageId = languageRequest.LanguageId,
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
            AddImageParentLink(links, languageRequest.LanguageId);
            AddDeleteBackgroundImageLink(links, languageRequest.LanguageId);
            AddSetBackgroundImageLink(links, languageRequest.LanguageId);
        }

        return result.ToActionResult(links: links);
    }
    
    [HttpDelete("{LanguageId:int}/background-image")]
    public async Task<ActionResult<ApiResponse>> DeleteBackgroundImage([FromRoute] LanguageRequest languageRequest, CancellationToken cancellationToken)
    {
        var result = await languageService.DeleteBackgroundImage(new DeleteLanguageBackgroundImageInput
        {
            LanguageId = languageRequest.LanguageId
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful)
        {
            AddImageParentLink(links, languageRequest.LanguageId);
            AddSetBackgroundImageLink(links, languageRequest.LanguageId);
        }

        return result.ToActionResult(links: links);
    }

    [HttpDelete("{LanguageId:int}")]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] LanguageRequest languageRequest, CancellationToken cancellationToken)
    {
        var result = await languageService.Delete(new DeleteLanguageInput
        {
            LanguageId = languageRequest.LanguageId
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful)
        {
            AddCollectionLink(links);
            AddCreateLink(links);
        }

        return result.ToActionResult(links: links);
    }

}
