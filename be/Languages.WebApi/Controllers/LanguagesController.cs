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
public sealed class LanguagesController(ILanguageService languageService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<GetManyLanguagesOutput?>>> GetMany([FromQuery] GetManyLanguagesRequest request, CancellationToken cancellationToken)
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

        var links = result.IsSuccessful && result.Data is not null
            ? GetPaginationLinks(result.Data, request)
            : null;

        return result.ToActionResult(links: links);
    }

    [HttpGet("{LanguageId:int}")]
    public async Task<ActionResult<ApiResponse<GetOneLanguageOutput?>>> GetOne([FromRoute] LanguageRequest languageRequest, CancellationToken cancellationToken)
    {
        var result = await languageService.GetOne(new GetOneLanguageInput
        {
            LanguageId = languageRequest.LanguageId
        }, cancellationToken);

        var links = result.IsSuccessful && result.Data is not null
            ? GetResourceLinks(result.Data.Id)
            : null;

        return result.ToActionResult(links: links);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CreateLanguageOutput>>> Create([FromForm] CreateLanguageRequest request, CancellationToken cancellationToken)
    {
        var result = await languageService.Create(new CreateLanguageInput
        {
            Name = request.Name,
        }, cancellationToken);

        var links = result.IsSuccessful && result.Data is not null
            ? GetResourceLinks(result.Data.Id)
            : null;

        return result.ToActionResult(statusCode: result.IsSuccessful ? HttpStatusCode.Created : null, links: links);
    }

    [HttpPatch("{LanguageId:int}")]
    public async Task<ActionResult<ApiResponse>> Update([FromRoute] LanguageRequest languageRequest, [FromBody] UpdateLanguageRequest request, CancellationToken cancellationToken)
    {
        var result = await languageService.Update(new UpdateLanguageInput
        {
            LanguageId = languageRequest.LanguageId,
            Name = request.Name,
        }, cancellationToken);

        var links = result.IsSuccessful ? GetResourceLinks(languageRequest.LanguageId) : null;

        return result.ToActionResult(links: links);
    }

    [HttpPut("{LanguageId:int}/background-image")]
    public async Task<ActionResult<ApiResponse<SetLanguageBackgroundImageOutput?>>> SetBackgroundImage([FromRoute] LanguageRequest languageRequest, [FromForm] SetLanguageBackgroundImageRequest request, CancellationToken cancellationToken)
    {
        using var imageStream = request.BackgroundImage.OpenReadStream();

        var result = await languageService.SetBackgroundImage(new SetLanguageBackgroundImageInput
        {
            LanguageId = languageRequest.LanguageId,
            Image = new SetImageInput
            {
                ContentType = request.BackgroundImage.ContentType,
                ImageStream = imageStream,
                Length = request.BackgroundImage.Length,
            }
        }, cancellationToken);

        var links = result.IsSuccessful ? GetResourceLinks(languageRequest.LanguageId) : null;

        return result.ToActionResult(links: links);
    }
    
    [HttpDelete("{LanguageId:int}/background-image")]
    public async Task<ActionResult<ApiResponse>> DeleteBackgroundImage([FromRoute] LanguageRequest languageRequest, CancellationToken cancellationToken)
    {
        var result = await languageService.DeleteBackgroundImage(new DeleteLanguageBackgroundImageInput
        {
            LanguageId = languageRequest.LanguageId
        }, cancellationToken);

        var links = result.IsSuccessful ? GetResourceLinks(languageRequest.LanguageId) : null;

        return result.ToActionResult(links: links);
    }

    [HttpDelete("{LanguageId:int}")]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] LanguageRequest languageRequest, CancellationToken cancellationToken)
    {
        var result = await languageService.Delete(new DeleteLanguageInput
        {
            LanguageId = languageRequest.LanguageId
        }, cancellationToken);

        var links = result.IsSuccessful ? GetCollectionLinks() : null;

        return result.ToActionResult(links: links);
    }

    private Dictionary<string, string> GetCollectionLinks() => new()
    {
        ["collection"] = Url.Action(nameof(GetMany), new { Page = 1, PageSize = 20 })!
    };

    private Dictionary<string, string> GetResourceLinks(int languageId)
    {
        var links = GetCollectionLinks();
        links["self"] = Url.Action(nameof(GetOne), new { LanguageId = languageId })!;
        links["dictionaries"] = Url.Action(nameof(DictionariesController.GetMany), "Dictionaries",
            new { LanguageId = languageId, Page = 1, PageSize = 20 })!;
        return links;
    }

    private Dictionary<string, string> GetPaginationLinks(GetManyOutput output, GetManyLanguagesRequest request)
    {
        string PageUrl(int page) => Url.Action(nameof(GetMany),
            new { Page = page, output.PageSize, request.Search })!;

        var links = new Dictionary<string, string> { ["self"] = PageUrl(output.Page) };

        if (output.Page > 1)
            links["prev"] = PageUrl(output.Page - 1);

        // Stay within the page limit enforced by GetManyInputValidator.
        if (output.Page < 10000 && (long)output.Page * output.PageSize < output.TotalCount)
            links["next"] = PageUrl(output.Page + 1);

        return links;
    }
}
