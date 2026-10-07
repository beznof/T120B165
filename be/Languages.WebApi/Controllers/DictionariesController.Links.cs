using System.Collections.ObjectModel;
using Languages.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Languages.WebApi.Controllers;

public sealed partial class DictionariesController
{
    private const int BASE_PAGE_SIZE = 100;
    private const int BASE_PAGE_NO = 1;
    
    private static object LanguageUrlParams(int languageId) => new { LanguageId = languageId };
    private static object DictionaryUrlParams(int languageId, int dictionaryId) => new { LanguageId = languageId, DictionaryId = dictionaryId };
    private static object EntriesPaginationUrlParams(int languageId, int dictionaryId, int page, int pageSize, string? search = null) => 
        new { LanguageId = languageId, DictionaryId = dictionaryId, Page = page, PageSize = pageSize, Search = search };
    private static object PaginationUrlParams(int languageId, int page, int pageSize, string? search = null) => 
        new { LanguageId = languageId, Page = page, PageSize = pageSize, Search = search };
    
    
    private string GetOneUrl(int languageId, int dictionaryId) => Url.Action(nameof(GetOne), DictionaryUrlParams(languageId, dictionaryId))
                                                                    ?? throw new InvalidOperationException("Could not generate a dictionary controller link.");
    private string GetManyUrl(int languageId, int page, int pageSize, string? search = null) => Url.Action(nameof(GetMany), PaginationUrlParams(languageId, page, pageSize, search))
                                                                    ?? throw new InvalidOperationException("Could not generate a dictionary controller link.");
    private string UpdateUrl(int languageId, int dictionaryId) => Url.Action(nameof(Update), DictionaryUrlParams(languageId, dictionaryId))
                                                                    ?? throw new InvalidOperationException("Could not generate a dictionary controller link.");
    private string DeleteUrl(int languageId, int dictionaryId) => Url.Action(nameof(Delete), DictionaryUrlParams(languageId, dictionaryId))
                                                                    ?? throw new InvalidOperationException("Could not generate a dictionary controller link.");
    private string CreateUrl(int languageId) => Url.Action(nameof(Create), LanguageUrlParams(languageId))
                                                                    ?? throw new InvalidOperationException("Could not generate a dictionary controller link.");
    private string SetBackgroundImageUrl(int languageId, int dictionaryId) => Url.Action(nameof(SetBackgroundImage), DictionaryUrlParams(languageId, dictionaryId))
                                                                    ?? throw new InvalidOperationException("Could not generate a dictionary controller link.");
    private string DeleteBackgroundImageUrl(int languageId, int dictionaryId) => Url.Action(nameof(DeleteBackgroundImage), DictionaryUrlParams(languageId, dictionaryId))
                                                                    ?? throw new InvalidOperationException("Could not generate a dictionary controller link.");
    private string GetOneLanguageUrl(int languageId) => Url.Action(nameof(LanguagesController.GetOne), "Languages", LanguageUrlParams(languageId))
                                                                    ?? throw new InvalidOperationException("Could not generate a dictionary controller link.");
    private string GetManyEntriesUrl(int languageId, int dictionaryId, int page, int pageSize) => Url.Action(nameof(EntriesController.GetMany), "Entries", EntriesPaginationUrlParams(languageId, dictionaryId, page, pageSize))
                                                                    ?? throw new InvalidOperationException("Could not generate a dictionary controller link.");

    
    private void AddSetBackgroundImageLink(Dictionary<string, string> parentDictionary, int languageId, int dictionaryId)
    {
        parentDictionary.Add("set-background-image", SetBackgroundImageUrl(languageId, dictionaryId));
    }
    
    private void AddDeleteBackgroundImageLink(Dictionary<string, string> parentDictionary, int languageId, int dictionaryId)
    {
        parentDictionary.Add("delete-background-image", DeleteBackgroundImageUrl(languageId, dictionaryId));
    }
    
    private void AddImageParentLink(Dictionary<string, string> parentDictionary, int languageId, int dictionaryId)
    {
        parentDictionary.Add("image-parent-dictionary", GetOneUrl(languageId, dictionaryId));
    }

    private void AddParentLanguageLink(Dictionary<string, string> parentDictionary, int languageId)
    {
        parentDictionary.Add("parent-language", GetOneLanguageUrl(languageId));
    }
    
    private void AddSelfResourceLinks(Dictionary<string, string> parentDictionary, int languageId, int dictionaryId)
    {
        parentDictionary.Add("self", GetOneUrl(languageId, dictionaryId));
        parentDictionary.Add("child-entries", GetManyEntriesUrl(languageId, dictionaryId, BASE_PAGE_NO, BASE_PAGE_SIZE));
        parentDictionary.Add("update", UpdateUrl(languageId, dictionaryId));
        parentDictionary.Add("delete", DeleteUrl(languageId, dictionaryId));
    }
    
    private void AddCreateLink(Dictionary<string, string> parentDictionary, int languageId)
    {
        parentDictionary.Add("create", CreateUrl(languageId));
    }

    private void AddParentLanguageCollectionLink(Dictionary<string, string> parentDictionary, int languageId)
    {
        parentDictionary.Add("dictionaries-collection", GetManyUrl(languageId, BASE_PAGE_NO, BASE_PAGE_SIZE));
    }

    private void AddPaginationLinks(Dictionary<string, string> parentDictionary, int languageId, int totalResultsCount,
        int page = BASE_PAGE_NO, int pageSize = BASE_PAGE_SIZE, string? search = null)
    {
        parentDictionary.Add("self", GetManyUrl(languageId, page, pageSize, search));
        
        if (page > 1)
        {
            parentDictionary.Add("first", GetManyUrl(languageId, BASE_PAGE_NO, pageSize, search));
            parentDictionary.Add("prev", GetManyUrl(languageId, page - 1, pageSize, search));
        }

        if (page < 10000 && page * pageSize < totalResultsCount)
            parentDictionary.Add("next", GetManyUrl(languageId, page + 1, pageSize, search));

        var lastPage = Math.Max((int)Math.Ceiling((double)totalResultsCount / pageSize), BASE_PAGE_NO);
        if (lastPage != page)
            parentDictionary.Add("last", GetManyUrl(languageId, lastPage, pageSize, search));
    }

}