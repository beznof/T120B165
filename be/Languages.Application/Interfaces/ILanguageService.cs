using Languages.Application.Models.Common;
using Languages.Application.Models.Inputs.Language;
using Languages.Application.Models.Outputs;

namespace Languages.Application.Interfaces;

public interface ILanguageService
{
    public Task<Result<GetManyLanguagesOutput?>> GetMany(GetManyLanguagesInput input, CancellationToken cancellationToken);
    public Task<Result<GetOneLanguageOutput?>> GetOne(GetOneLanguageInput input, CancellationToken cancellationToken);
    public Task<Result<CreateLanguageOutput?>> Create(CreateLanguageInput input, CancellationToken cancellationToken);
    public Task<Result> Update(UpdateLanguageInput input, CancellationToken cancellationToken);
    public Task<Result> Delete(DeleteLanguageInput input, CancellationToken cancellationToken);
    public Task<Result<SetLanguageBackgroundImageOutput?>> SetBackgroundImage(SetLanguageBackgroundImageInput input, CancellationToken cancellationToken);
    public Task<Result> DeleteBackgroundImage(DeleteLanguageBackgroundImageInput input, CancellationToken cancellationToken);
}
