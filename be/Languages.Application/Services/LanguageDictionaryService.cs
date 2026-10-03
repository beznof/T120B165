using Languages.Application.Interfaces;
using Languages.Application.Models.Common;
using Languages.Application.Models.LanguageDictionary;
using Languages.Application.Persistence;
using Languages.Domain.Entities;

namespace Languages.Application.Services;

public class LanguageDictionaryService(ILanguagesDbContext dbContext) : ILanguageDictionaryService
{
    private readonly ILanguagesDbContext _dbContext = dbContext;
    
    public async Task<Result<IReadOnlyList<LanguageDictionary>>> GetAllLanguagesDictionaries(ReadManyInput readManyInput)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result<LanguageDictionary>> GetLanguageDictionary(int languageId, int languageDictionaryId)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result<LanguageDictionary>> CreateLanguageDictionary(int languageId, int languageDictionaryId, CreateLanguageDictionaryInput createLanguageDictionaryInput)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result> UpdateLanguage(int languageId, int languageDictionaryId, UpdateLanguageDictionaryInput updateLanguageDictionaryInput)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result> DeleteLanguage(int languageId, int languageDictionaryId)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result> SetLanguageBackgroundImage(int languageId, int languageDictionaryId, SetImageInput setImageInput)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result> DeleteLanguageBackgroundImage(int languageId, int languageDictionaryId)
    {
        throw new NotImplementedException();
    }
}