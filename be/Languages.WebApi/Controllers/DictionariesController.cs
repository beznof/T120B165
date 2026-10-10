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

/// <summary>Dictionary management.</summary>
[ApiController]
[Route("api/languages/{LanguageId}/dictionaries")]
[Produces("application/json")]
public sealed partial class DictionariesController(IDictionaryService dictionaryService) : ControllerBase
{
    /// <summary>List dictionaries.</summary>
    /// <remarks>
    /// List dictionaries within the specified language in paginated manner by supplying the page size, the page number, and optionally a name search filter.
    /// </remarks>
    /// <returns>The requested page of dictionaries list.</returns>
    /// <response code="200">The requested page of dictionaries was retrieved successfully.</response>
    /// <response code="400">The path or query parameters were missing or incorrect.</response>
    /// <response code="404">The requested language was not found.</response>
    [HttpGet]
    [ProducesResponseType<ApiResponse<GetManyDictionariesOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetManyDictionariesOutput>>> GetMany([FromRoute] BaseDictionaryRequest dictionaryRequest, [FromQuery] GetManyDictionariesRequest request, CancellationToken cancellationToken)
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

    /// <summary>Get a dictionary by its identifier.</summary>
    /// <remarks>
    /// Get a dictionary by its identifier within the specified language.
    /// </remarks>
    /// <response code="200">The dictionary was retrieved successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect.</response>
    /// <response code="404">The requested dictionary was not found in the specified language.</response>
    [HttpGet("{DictionaryId}")]
    [ProducesResponseType<ApiResponse<GetOneDictionaryOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetOneDictionaryOutput>>> GetOne([FromRoute] DictionaryRequest dictionaryRequest, CancellationToken cancellationToken)
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

    /// <summary>Create a dictionary.</summary>
    /// <remarks>
    /// Create a dictionary within the specified language by specifying its properties.
    /// </remarks>
    /// <response code="201">The dictionary was created successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect or the JSON body was malformed or incorrect.</response>
    /// <response code="404">The requested language was not found.</response>
    [HttpPost]
    [ProducesResponseType<ApiResponse<CreateDictionaryOutput>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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

    /// <summary>Update a dictionary.</summary>
    /// <remarks>
    /// Modify a dictionary, indicated by its identifier, within the specified language by specifying its new property values.
    /// </remarks>
    /// <response code="200">The dictionary was updated successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect or the JSON body was malformed or incorrect.</response>
    /// <response code="404">The requested dictionary was not found in the specified language.</response>
    [HttpPatch("{DictionaryId}")]
    [ProducesResponseType<ApiResponse<UpdateDictionaryOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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

    /// <summary>Set the dictionary's background image.</summary>
    /// <remarks>
    /// Set the dictionary's, indicated by its identifier, background image.
    /// </remarks>
    /// <response code="200">The background image was uploaded successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect or the uploaded image was invalid.</response>
    /// <response code="404">The requested dictionary was not found in the specified language.</response>
    [HttpPut("{DictionaryId}/background-image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType<ApiResponse<SetDictionaryBackgroundImageOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SetDictionaryBackgroundImageOutput>>> SetBackgroundImage([FromRoute] DictionaryRequest dictionaryRequest, [FromForm] SetDictionaryBackgroundImageRequest request, CancellationToken cancellationToken)
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
    
    /// <summary>Delete the dictionary's background image.</summary>
    /// <remarks>
    /// Delete the dictionary's, indicated by its identifier, background image.
    /// </remarks>
    /// <response code="200">The dictionary's background image was deleted successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect.</response>
    /// <response code="404">The requested dictionary was not found in the specified language or it didn't have a background image set.</response>
    [HttpDelete("{DictionaryId}/background-image")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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

    /// <summary>Delete a dictionary.</summary>
    /// <remarks>
    /// Delete a dictionary, indicated by its identifier, within the specified language.
    /// </remarks>
    /// <response code="200">The dictionary was deleted successfully.</response>
    /// <response code="400">The path parameters were missing or incorrect.</response>
    /// <response code="404">The requested dictionary was not found in the specified language.</response>
    [HttpDelete("{DictionaryId}")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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
