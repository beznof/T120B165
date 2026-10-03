using Languages.WebApi.Models.Common;
using Languages.WebApi.Models.Language.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Controllers;

[ApiController]
[Route("api/languages")]
public sealed class LanguagesController : ControllerBase
{
    [HttpGet]
    public IActionResult ReadMany([FromQuery] ReadManyLanguagesRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{LanguageId:int}")]
    public IActionResult Read([FromRoute] LanguageRequest languageRequest)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult Create([FromForm] CreateLanguageRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{LanguageId:int}")]
    public IActionResult Update([FromRoute] LanguageRequest languageRequest, [FromBody] UpdateLanguageRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{LanguageId:int}/background-image")]
    public IActionResult SetBackgroundImage([FromRoute] LanguageRequest languageRequest, [FromForm] SetLanguageBackgroundImageRequest request)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("{DictionaryId:int}/background-image")]
    public async Task<ActionResult<ApiResponse>> DeleteBackgroundImage([FromRoute] LanguageRequest languageRequest)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{LanguageId:int}")]
    public IActionResult Delete([FromRoute] LanguageRequest languageRequest)
    {
        throw new NotImplementedException();
    }
}
