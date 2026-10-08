using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Controllers;

public sealed partial class LanguagesController
{
    private const int BasePageSize = 100;
    private const int BasePageNo = 1;

    private static object LanguageUrlParams(int languageId) => new { LanguageId = languageId };
    private static object PaginationUrlParams(int page, int pageSize, string? search = null) => new { Page = page, PageSize = pageSize, Search = search };
    private static object DictionariesPaginationUrlParams(int languageId, int page, int pageSize) => new { LanguageId = languageId, Page = page, PageSize = pageSize };

    
    private string GetOneUrl(int languageId) => Url.Action(nameof(GetOne), LanguageUrlParams(languageId))
                                            ?? throw new InvalidOperationException("Could not generate a language controller link.");
    private string GetManyUrl(int page, int pageSize, string? search = null) => Url.Action(nameof(GetMany), PaginationUrlParams(page, pageSize, search))
                                            ?? throw new InvalidOperationException("Could not generate a language controller link.");
    private string CreateUrl() => Url.Action(nameof(Create))
                                            ?? throw new InvalidOperationException("Could not generate a language controller link.");
    private string UpdateUrl(int languageId) => Url.Action(nameof(Update), LanguageUrlParams(languageId))
                                            ?? throw new InvalidOperationException("Could not generate a language controller link.");
    private string DeleteUrl(int languageId) => Url.Action(nameof(Delete), LanguageUrlParams(languageId))
                                            ?? throw new InvalidOperationException("Could not generate a language controller link.");
    private string SetBackgroundImageUrl(int languageId) => Url.Action(nameof(SetBackgroundImage), LanguageUrlParams(languageId))
                                            ?? throw new InvalidOperationException("Could not generate a language controller link.");
    private string DeleteBackgroundImageUrl(int languageId) => Url.Action(nameof(DeleteBackgroundImage), LanguageUrlParams(languageId))
                                            ?? throw new InvalidOperationException("Could not generate a language controller link.");
    private string GetManyDictionariesUrl(int languageId, int page, int pageSize) => Url.Action(nameof(DictionariesController.GetMany), "Dictionaries", DictionariesPaginationUrlParams(languageId, page, pageSize))
                                            ?? throw new InvalidOperationException("Could not generate a language controller link.");

    
    private void AddSetBackgroundImageLink(Dictionary<string, string> links, int languageId)
    {
        links.Add("set-background-image", SetBackgroundImageUrl(languageId));
    }

    private void AddDeleteBackgroundImageLink(Dictionary<string, string> links, int languageId)
    {
        links.Add("delete-background-image", DeleteBackgroundImageUrl(languageId));
    }

    private void AddImageParentLink(Dictionary<string, string> links, int languageId)
    {
        links.Add("image-parent-language", GetOneUrl(languageId));
    }

    private void AddSelfResourceLinks(Dictionary<string, string> links, int languageId)
    {
        links.Add("self", GetOneUrl(languageId));
        links.Add("child-dictionaries", GetManyDictionariesUrl(languageId, BasePageNo, BasePageSize));
        links.Add("update", UpdateUrl(languageId));
        links.Add("delete", DeleteUrl(languageId));
    }

    private void AddCreateLink(Dictionary<string, string> links)
    {
        links.Add("create", CreateUrl());
    }

    private void AddCollectionLink(Dictionary<string, string> links)
    {
        links.Add("languages-collection", GetManyUrl(BasePageNo, BasePageSize));
    }

    private void AddPaginationLinks(Dictionary<string, string> links, int totalResultsCount,
        int page = BasePageNo, int pageSize = BasePageSize, string? search = null)
    {
        links.Add("self", GetManyUrl(page, pageSize, search));

        if (page > 1)
        {
            links.Add("first", GetManyUrl(BasePageNo, pageSize, search));
            links.Add("prev", GetManyUrl(page - 1, pageSize, search));
        }

        if (page < 10000 && (long)page * pageSize < totalResultsCount)
            links.Add("next", GetManyUrl(page + 1, pageSize, search));

        var lastPage = Math.Clamp((int)Math.Ceiling((double)totalResultsCount / pageSize), BasePageNo, 10000);
        if (lastPage != page)
            links.Add("last", GetManyUrl(lastPage, pageSize, search));
    }
}
