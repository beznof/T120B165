using Languages.Application.Interfaces;
using Languages.WebApi.Models.Common;
using Languages.WebApi.Models.Requests.LanguageDictionary;
using Languages.WebApi.Models.Responses.LanguageDictionary;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Controllers;

[ApiController]
[Route("api/languages/{LanguageId:int}/dictionaries")]
public sealed class LanguageDictionariesController(IDictionaryService dictionaryService) : ControllerBase
{
    private readonly IDictionaryService _dictionaryService = dictionaryService;
    
    [HttpGet]
    public async Task<ActionResult<ApiResponse<ReadManyLanguageDictionariesResponse>>> ReadMany([FromRoute] BaseLanguageDictionaryRequest dictionaryRequest, [FromQuery] ReadManyLanguageDictionariesRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{DictionaryId:int}")]
    public async Task<ActionResult<ApiResponse<ReadLanguageDictionaryResponse>>> Read([FromRoute] LanguageDictionaryRequest dictionaryRequest)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CreateLanguageDictionaryResponse>>> Create([FromRoute] BaseLanguageDictionaryRequest dictionaryRequest, [FromBody] CreateLanguageDictionaryRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{DictionaryId:int}")]
    public async Task<ActionResult<ApiResponse>> Update([FromRoute] LanguageDictionaryRequest dictionaryRequest, [FromBody] UpdateLanguageDictionaryRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{DictionaryId:int}/background-image")]
    public async Task<ActionResult<ApiResponse>> SetBackgroundImage([FromRoute] LanguageDictionaryRequest dictionaryRequest, [FromForm] SetLanguageDictionaryBackgroundImageRequest request)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("{DictionaryId:int}/background-image")]
    public async Task<ActionResult<ApiResponse>> DeleteBackgroundImage([FromRoute] LanguageDictionaryRequest dictionaryRequest)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{DictionaryId:int}")]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] LanguageDictionaryRequest dictionaryRequest)
    {
        throw new NotImplementedException();
    }
}
