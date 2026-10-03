using Languages.Application.Interfaces;
using Languages.WebApi.Models.Common;
using Languages.WebApi.Models.Requests.DictionaryEntry;
using Languages.WebApi.Models.Responses.DictionaryEntry;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Controllers;

[ApiController]
[Route("api/languages/{LanguageId:int}/dictionaries/{DictionaryId:int}/entries")]
public sealed class DictionaryEntriesController(IDictionaryEntryService dictionaryEntryService) : ControllerBase
{
    private readonly IDictionaryEntryService _dictionaryEntryService = dictionaryEntryService;
    
    [HttpGet]
    public async Task<ActionResult<ApiResponse<ReadManyDictionaryEntriesResponse>>> ReadMany([FromRoute] BaseDictionaryEntryRequest entryRequest, [FromQuery] ReadManyDictionaryEntriesRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{DictionaryEntryId:int}")]
    public async Task<ActionResult<ApiResponse<ReadDictionaryEntryResponse>>> Read([FromRoute] DictionaryEntryRequest entryRequest)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CreateDictionaryEntryResponse>>> Create([FromRoute] BaseDictionaryEntryRequest entryRequest, CreateDictionaryEntryRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{DictionaryEntryId:int}")]
    public async Task<ActionResult<ApiResponse>> Update([FromRoute] DictionaryEntryRequest entryRequest, [FromBody] UpdateDictionaryEntryRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{DictionaryEntryId:int}")]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] DictionaryEntryRequest entryRequest)
    {
        throw new NotImplementedException();
    }
}
