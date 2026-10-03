using Languages.Application.Models.Common;
using Languages.Application.Models.Language;
using Languages.Domain.Entities;

namespace Languages.Application.Interfaces;

public interface ILanguageService
{
    public Task<Result<IReadOnlyList<Language>>> GetAllLanguages(ReadManyInput readManyInput);
    public Task<Result<Language>> GetLanguage(int languageId);
    public Task<Language> CreateLanguage(CreateLanguageInput createLanguageInput);
    public Task<Result> UpdateLanguage(int languageId, UpdateLanguageInput updateLanguageInput);
    public Task<Result> DeleteLanguage(int languageId);
    public Task<Result> SetLanguageBackgroundImage(int languageId, Stream imageStream);
    public Task<Result> DeleteLanguageBackgroundImage(int languageId);
}
 