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

/// <summary>Language management.</summary>
[ApiController]
[Route("api/languages")]
[Produces("application/json")]
public sealed partial class LanguagesController(ILanguageService languageService) : ControllerBase
{
    /// <summary>List languages.</summary>
    /// <remarks>
    /// List languages in paginated manner by supplying the page size, the page number, and optionally a name search filter.
    /// </remarks>
    /// <returns>The requested page of languages list.</returns>
    /// <response code="200">The requested page of languages was retrieved successfully.</response>
    /// <response code="400">The query parameters were missing or incorrect.</response>
    [HttpGet]
    [ProducesResponseType<ApiResponse<GetManyLanguagesOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
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

    /// <summary>Get a language by its identifier.</summary>
    /// <remarks>
    /// Get a language by its identifier.
    /// </remarks>
    /// <response code="200">The language was retrieved successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect.</response>
    /// <response code="404">The requested language was not found.</response>
    [HttpGet("{LanguageId}")]
    [ProducesResponseType<ApiResponse<GetOneLanguageOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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

    /// <summary>Create a language.</summary>
    /// <remarks>
    /// Create a language by specifying its properties.
    /// </remarks>
    /// <response code="201">The language was created successfully.</response>
    /// <response code="400">The JSON body was malformed or incorrect.</response>
    [HttpPost]
    [ProducesResponseType<ApiResponse<CreateLanguageOutput>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<CreateLanguageOutput>>> Create([FromBody] CreateLanguageRequest request, CancellationToken cancellationToken)
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

    /// <summary>Update a language.</summary>
    /// <remarks>
    /// Modify a language, indicated by its identifier, by specifying its new property values.
    /// </remarks>
    /// <response code="200">The language was updated successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect or the JSON body was malformed or incorrect.</response>
    /// <response code="404">The requested language was not found.</response>
    [HttpPatch("{LanguageId}")]
    [ProducesResponseType<ApiResponse<UpdateLanguageOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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

    /// <summary>Set the language's background image.</summary>
    /// <remarks>
    /// Set the language's, indicated by its identifier, background image.
    /// </remarks>
    /// <response code="200">The background image was uploaded successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect or the uploaded image was invalid.</response>
    /// <response code="404">The requested language was not found.</response>
    [HttpPut("{LanguageId}/background-image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType<ApiResponse<SetLanguageBackgroundImageOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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
    
    /// <summary>Delete the language's background image.</summary>
    /// <remarks>
    /// Delete the language's, indicated by its identifier, background image.
    /// </remarks>
    /// <response code="200">The language's background image was deleted successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect.</response>
    /// <response code="404">The requested language was not found or it didn't have a background image set.</response>
    [HttpDelete("{LanguageId}/background-image")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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

    /// <summary>Delete a language.</summary>
    /// <remarks>
    /// Delete a language, indicated by its identifier.
    /// </remarks>
    /// <response code="200">The language was deleted successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect.</response>
    /// <response code="404">The requested language was not found.</response>
    [HttpDelete("{LanguageId}")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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
