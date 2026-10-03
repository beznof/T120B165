using Languages.Application.Models.Common;
using Languages.Application.Models.LanguageDictionary;
using Languages.Domain.Entities;

namespace Languages.Application.Interfaces;

public interface ILanguageDictionaryService
{
    public Task<Result<IReadOnlyList<LanguageDictionary>>> GetAllLanguagesDictionaries(ReadManyInput readManyInput);
    public Task<Result<LanguageDictionary>> GetLanguageDictionary(int languageId, int languageDictionaryId);
    public Task<Result<LanguageDictionary>> CreateLanguageDictionary(int languageId, int languageDictionaryId, CreateLanguageDictionaryInput createLanguageDictionaryInput);
    public Task<Result> UpdateLanguage(int languageId, int languageDictionaryId, UpdateLanguageDictionaryInput updateLanguageDictionaryInput);
    public Task<Result> DeleteLanguage(int languageId, int languageDictionaryId);
    public Task<Result> SetLanguageBackgroundImage(int languageId, int languageDictionaryId, SetImageInput setImageInput);
    public Task<Result> DeleteLanguageBackgroundImage(int languageId, int languageDictionaryId);
}