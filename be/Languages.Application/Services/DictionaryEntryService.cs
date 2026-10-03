using Languages.Application.Interfaces;
using Languages.Application.Models.Common;
using Languages.Application.Models.DictionaryEntry;
using Languages.Application.Persistence;
using Languages.Domain.Entities;
using Languages.Domain.Enums;

namespace Languages.Application.Services;

public class DictionaryEntryService(ILanguagesDbContext dbContext) : IDictionaryEntryService
{
    private readonly ILanguagesDbContext _dbContext = dbContext;
    
    public async Task<Result<IReadOnlyList<DictionaryEntry>>> GetAllLanguagesDictionaries(ReadManyInput readManyInput, DictionaryEntryType? dictionaryEntryType = null)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<DictionaryEntry>> GetLanguageDictionary(int languageId, int languageDictionaryId, int dictionaryEntryId)
    {
        throw new NotImplementedException();    
    }

    public async Task<Result<DictionaryEntry>> CreateLanguageDictionary(int languageId, int languageDictionaryId, CreateDictionaryEntryInput createLanguageDictionaryInput)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateLanguage(int languageId, int languageDictionaryId, int dictionaryEntryId, UpdateDictionaryEntryInput updateLanguageDictionaryInput)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteLanguage(int languageId, int languageDictionaryId, int dictionaryEntryId)
    {
        throw new NotImplementedException();
    }
}