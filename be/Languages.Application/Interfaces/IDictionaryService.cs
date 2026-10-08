using Languages.Application.Models.Common;
using Languages.Application.Models.Inputs.Dictionary;
using Languages.Application.Models.Outputs;

namespace Languages.Application.Interfaces;

public interface IDictionaryService
{
    public Task<Result<GetManyDictionariesOutput?>> GetMany(GetManyDictionariesInput input, CancellationToken cancellationToken);
    public Task<Result<GetOneDictionaryOutput?>> GetOne(GetOneDictionaryInput input, CancellationToken cancellationToken);
    public Task<Result<CreateDictionaryOutput?>> Create(CreateDictionaryInput input, CancellationToken cancellationToken);
    public Task<Result<UpdateDictionaryOutput>> Update(UpdateDictionaryInput input, CancellationToken cancellationToken);
    public Task<Result> Delete(DeleteDictionaryInput input, CancellationToken cancellationToken);
    public Task<Result<SetDictionaryBackgroundImageOutput?>> SetBackgroundImage(SetDictionaryBackgroundImageInput input, CancellationToken cancellationToken);
    public Task<Result> DeleteBackgroundImage(DeleteDictionaryBackgroundImageInput input, CancellationToken cancellationToken);
}
