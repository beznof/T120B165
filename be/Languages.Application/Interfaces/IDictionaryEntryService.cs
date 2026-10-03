using Languages.Application.Models.Common;
using Languages.Application.Models.DictionaryEntry;
using Languages.Domain.Entities;
using Languages.Domain.Enums;

namespace Languages.Application.Interfaces;

public interface IDictionaryEntryService
{
    public Task<Result<IReadOnlyList<DictionaryEntry>>> GetAllLanguagesDictionaries(ReadManyInput readManyInput, DictionaryEntryType? dictionaryEntryType = null);
    public Task<Result<DictionaryEntry>> GetLanguageDictionary(int languageId, int languageDictionaryId, int dictionaryEntryId);
    public Task<Result<DictionaryEntry>> CreateLanguageDictionary(int languageId, int languageDictionaryId, CreateDictionaryEntryInput createLanguageDictionaryInput);
    public Task<Result> UpdateLanguage(int languageId, int languageDictionaryId, int dictionaryEntryId, UpdateDictionaryEntryInput updateLanguageDictionaryInput);
    public Task<Result> DeleteLanguage(int languageId, int languageDictionaryId, int dictionaryEntryId);
}