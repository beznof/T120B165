using System.Net;
using System.Security.Claims;
using Languages.Application.Interfaces;
using Languages.Application.Models.Inputs.Common;
using Languages.Application.Models.Inputs.Dictionary;
using Languages.Application.Models.Outputs;
using Languages.Domain.Enums;
using Languages.WebApi.Extensions;
using Languages.WebApi.Models.Common;
using Languages.WebApi.Models.Requests.Dictionary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Controllers;

/// <summary>Dictionary management.</summary>
[ApiController]
[Route("api/languages/{LanguageId}/dictionaries")]
[Produces("application/json")]
[Authorize(Roles = $"{nameof(UserRole.Learner)},{nameof(UserRole.Teacher)},{nameof(UserRole.Admin)}")]
public sealed partial class DictionariesController(IDictionaryService dictionaryService) : ControllerBase
{
    private int UserId => int.TryParse(User.FindFirstValue("sub"), out var userId) 
        ? userId 
        : throw new InvalidDataException("Couldn't retrieve the user's ID");
    private bool IsAdmin => User.IsInRole(nameof(UserRole.Admin));
    
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
        var languageId = dictionaryRequest.LanguageId!.Value;
        var pageSize = request.PageSize!.Value;
        var page = request.Page!.Value;
        var search = request.Search;

        var result = await dictionaryService.GetMany(new GetManyDictionariesInput
        {
            LanguageId = languageId,
            Pagination = new GetManyInput
            {
                PageSize = pageSize,
                Page = page,
                Search = search,
            }
        }, cancellationToken);

        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentLanguageLink(links, languageId);
            AddPaginationLinks(links, languageId, result.Data.TotalCount, page, pageSize, search);
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
        var languageId = dictionaryRequest.LanguageId!.Value;
        var dictionaryId = dictionaryRequest.DictionaryId!.Value;

        var result = await dictionaryService.GetOne(new GetOneDictionaryInput
        {
            LanguageId = languageId,
            DictionaryId = dictionaryId
        }, cancellationToken);
        
        var links = new Dictionary<string, string>();
        if (result.IsSuccessful && result.Data != null)
        {
            AddParentLanguageLink(links, languageId);
            AddSelfResourceLinks(links, languageId, dictionaryId);
            AddSetBackgroundImageLink(links, languageId, dictionaryId);
            if (!string.IsNullOrWhiteSpace(result.Data.BackgroundImageUrl))
                AddDeleteBackgroundImageLink(links, languageId, dictionaryId);
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
    [Authorize(Roles = $"{nameof(UserRole.Teacher)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType<ApiResponse<CreateDictionaryOutput>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CreateDictionaryOutput>>> Create([FromRoute] BaseDictionaryRequest dictionaryRequest, [FromBody] CreateDictionaryRequest request, CancellationToken cancellationToken)
    {
        var languageId = dictionaryRequest.LanguageId!.Value;
        var name = request.Name;

        var result = await dictionaryService.Create(new CreateDictionaryInput
            {
                LanguageId = languageId,
                Name = name,
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
            AddParentLanguageLink(links, languageId);
            AddSelfResourceLinks(links, languageId, result.Data.Id);
            AddSetBackgroundImageLink(links, languageId, result.Data.Id);
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
    [Authorize(Roles = $"{nameof(UserRole.Teacher)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType<ApiResponse<UpdateDictionaryOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UpdateDictionaryOutput>>> Update([FromRoute] DictionaryRequest dictionaryRequest, [FromBody] UpdateDictionaryRequest request, CancellationToken cancellationToken)
    {
        var languageId = dictionaryRequest.LanguageId!.Value;
        var dictionaryId = dictionaryRequest.DictionaryId!.Value;
        var name = request.Name;

        var result = await dictionaryService.Update(new UpdateDictionaryInput
            {
                LanguageId = languageId,
                DictionaryId = dictionaryId,
                Name = name,
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
            AddParentLanguageLink(links, languageId);
            AddSelfResourceLinks(links, languageId, dictionaryId);
            AddSetBackgroundImageLink(links, languageId, dictionaryId);
            if (!string.IsNullOrWhiteSpace(result.Data.BackgroundImageUrl))
                AddDeleteBackgroundImageLink(links, languageId, dictionaryId);
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
    [Authorize(Roles = $"{nameof(UserRole.Teacher)},{nameof(UserRole.Admin)}")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType<ApiResponse<SetDictionaryBackgroundImageOutput>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SetDictionaryBackgroundImageOutput>>> SetBackgroundImage([FromRoute] DictionaryRequest dictionaryRequest, [FromForm] SetDictionaryBackgroundImageRequest request, CancellationToken cancellationToken)
    {
        var languageId = dictionaryRequest.LanguageId!.Value;
        var dictionaryId = dictionaryRequest.DictionaryId!.Value;
        var backgroundImage = request.BackgroundImage!;

        var result = await dictionaryService.SetBackgroundImage(new SetDictionaryBackgroundImageInput
            {
                LanguageId = languageId,
                DictionaryId = dictionaryId,
                Image = new SetImageInput
                {
                    ContentType = backgroundImage.ContentType,
                    ImageStream = backgroundImage.OpenReadStream(),
                    Length = backgroundImage.Length,
                }
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
            AddImageParentLink(links, languageId, dictionaryId);
            AddDeleteBackgroundImageLink(links, languageId, dictionaryId);
            AddSetBackgroundImageLink(links, languageId, dictionaryId);
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
    [Authorize(Roles = $"{nameof(UserRole.Teacher)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> DeleteBackgroundImage([FromRoute] DictionaryRequest dictionaryRequest, CancellationToken cancellationToken)
    {
        var languageId = dictionaryRequest.LanguageId!.Value;
        var dictionaryId = dictionaryRequest.DictionaryId!.Value;

        var result = await dictionaryService.DeleteBackgroundImage(new DeleteDictionaryBackgroundImageInput
            {
                LanguageId = languageId,
                DictionaryId = dictionaryId
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
            AddImageParentLink(links, languageId, dictionaryId);
            AddSetBackgroundImageLink(links, languageId, dictionaryId);
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
    [Authorize(Roles = $"{nameof(UserRole.Teacher)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] DictionaryRequest dictionaryRequest, CancellationToken cancellationToken)
    {
        var languageId = dictionaryRequest.LanguageId!.Value;
        var dictionaryId = dictionaryRequest.DictionaryId!.Value;

        var result = await dictionaryService.Delete(new DeleteDictionaryInput
            {
                LanguageId = languageId,
                DictionaryId = dictionaryId
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
            AddParentLanguageLink(links, languageId);
            AddParentLanguageCollectionLink(links, languageId);
            AddCreateLink(links, languageId);
        }

        return result.ToActionResult(links: links);
    }
}
