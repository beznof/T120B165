using Languages.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Controllers;

public sealed partial class EntriesController
{
    private const int BasePageSize = 100;
    private const int BasePageNo = 1;

    private static object DictionaryUrlParams(int languageId, int dictionaryId) => new { LanguageId = languageId, DictionaryId = dictionaryId };
    private static object EntryUrlParams(int languageId, int dictionaryId, int entryId) => new { LanguageId = languageId, DictionaryId = dictionaryId, EntryId = entryId };
    private static object PaginationUrlParams(int languageId, int dictionaryId, int page, int pageSize, string? search = null, DictionaryEntryType? type = null) =>
        new { LanguageId = languageId, DictionaryId = dictionaryId, Page = page, PageSize = pageSize, Search = search, Type = type };

    
    private string GetOneUrl(int languageId, int dictionaryId, int entryId) => Url.Action(nameof(GetOne), EntryUrlParams(languageId, dictionaryId, entryId))
                                                                                ?? throw new InvalidOperationException("Could not generate an entry controller link.");
    private string GetManyUrl(int languageId, int dictionaryId, int page, int pageSize, string? search = null, DictionaryEntryType? type = null) =>
                                                                                Url.Action(nameof(GetMany), PaginationUrlParams(languageId, dictionaryId, page, pageSize, search, type))
                                                                                ?? throw new InvalidOperationException("Could not generate an entry controller link.");
    private string CreateUrl(int languageId, int dictionaryId) => Url.Action(nameof(Create), DictionaryUrlParams(languageId, dictionaryId))
                                                                                ?? throw new InvalidOperationException("Could not generate an entry controller link.");
    private string UpdateUrl(int languageId, int dictionaryId, int entryId) => Url.Action(nameof(Update), EntryUrlParams(languageId, dictionaryId, entryId))
                                                                                ?? throw new InvalidOperationException("Could not generate an entry controller link.");
    private string DeleteUrl(int languageId, int dictionaryId, int entryId) => Url.Action(nameof(Delete), EntryUrlParams(languageId, dictionaryId, entryId))
                                                                                ?? throw new InvalidOperationException("Could not generate an entry controller link.");
    private string GetOneDictionaryUrl(int languageId, int dictionaryId) => Url.Action(nameof(DictionariesController.GetOne), "Dictionaries", DictionaryUrlParams(languageId, dictionaryId))
                                                                                ?? throw new InvalidOperationException("Could not generate an entry controller link.");

    
    private void AddParentDictionaryLink(Dictionary<string, string> parentDictionary, int languageId, int dictionaryId)
    {
        parentDictionary.Add("parent-dictionary", GetOneDictionaryUrl(languageId, dictionaryId));
    }

    private void AddSelfResourceLinks(Dictionary<string, string> parentDictionary, int languageId, int dictionaryId, int entryId)
    {
        parentDictionary.Add("self", GetOneUrl(languageId, dictionaryId, entryId));
        parentDictionary.Add("update", UpdateUrl(languageId, dictionaryId, entryId));
        parentDictionary.Add("delete", DeleteUrl(languageId, dictionaryId, entryId));
    }

    private void AddCreateLink(Dictionary<string, string> parentDictionary, int languageId, int dictionaryId)
    {
        parentDictionary.Add("create", CreateUrl(languageId, dictionaryId));
    }

    private void AddParentDictionaryCollectionLink(Dictionary<string, string> parentDictionary, int languageId, int dictionaryId)
    {
        parentDictionary.Add("entries-collection", GetManyUrl(languageId, dictionaryId, BasePageNo, BasePageSize));
    }

    private void AddPaginationLinks(Dictionary<string, string> parentDictionary, int languageId, int dictionaryId,
        int totalResultsCount, int page = BasePageNo, int pageSize = BasePageSize, string? search = null,
        DictionaryEntryType? type = null)
    {
        parentDictionary.Add("self", GetManyUrl(languageId, dictionaryId, page, pageSize, search, type));

        if (page > 1)
        {
            parentDictionary.Add("first", GetManyUrl(languageId, dictionaryId, BasePageNo, pageSize, search, type));
            parentDictionary.Add("prev", GetManyUrl(languageId, dictionaryId, page - 1, pageSize, search, type));
        }

        if (page < 10000 && (long)page * pageSize < totalResultsCount)
            parentDictionary.Add("next", GetManyUrl(languageId, dictionaryId, page + 1, pageSize, search, type));

        var lastPage = Math.Clamp((int)Math.Ceiling((double)totalResultsCount / pageSize), BasePageNo, 10000);
        if (lastPage != page)
            parentDictionary.Add("last", GetManyUrl(languageId, dictionaryId, lastPage, pageSize, search, type));
    }
}
