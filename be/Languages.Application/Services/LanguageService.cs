using Languages.Application.Interfaces;
using Languages.Application.Models.Common;
using Languages.Application.Models.Language;
using Languages.Application.Persistence;
using Languages.Domain.Entities;

namespace Languages.Application.Services;

public class LanguageService(ILanguagesDbContext dbContext) : ILanguageService
{
    private readonly ILanguagesDbContext _dbContext = dbContext;

    public async Task<Result<IReadOnlyList<Language>>> GetAllLanguages(ReadManyInput readManyInput)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result<Language>> GetLanguage(int languageId)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Language> CreateLanguage(CreateLanguageInput createLanguageInput)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result> UpdateLanguage(int languageId, UpdateLanguageInput updateLanguageInput)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result> DeleteLanguage(int languageId)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result> SetLanguageBackgroundImage(int languageId, Stream imageStream)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result> DeleteLanguageBackgroundImage(int languageId)
    {
        throw new NotImplementedException();
    }
}