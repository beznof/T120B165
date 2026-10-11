using Languages.Application.Models.Common;
using Languages.Application.Models.Inputs.Common;
using Languages.Application.Models.Inputs.Dictionary;
using Languages.Application.Models.Outputs;

namespace Languages.Application.Interfaces;

public interface IDictionaryService
{
    public Task<Result<GetManyDictionariesOutput>> GetMany(GetManyDictionariesInput input, CancellationToken cancellationToken);
    public Task<Result<GetOneDictionaryOutput>> GetOne(GetOneDictionaryInput input, CancellationToken cancellationToken);
    public Task<Result<CreateDictionaryOutput>> Create(CreateDictionaryInput input, AuthorInput authorInput,  CancellationToken cancellationToken);
    public Task<Result<UpdateDictionaryOutput>> Update(UpdateDictionaryInput input, AuthorInput authorInput, CancellationToken cancellationToken);
    public Task<Result> Delete(DeleteDictionaryInput input, AuthorInput authorInput, CancellationToken cancellationToken);
    public Task<Result<SetDictionaryBackgroundImageOutput>> SetBackgroundImage(SetDictionaryBackgroundImageInput input, AuthorInput authorInput, CancellationToken cancellationToken);
    public Task<Result> DeleteBackgroundImage(DeleteDictionaryBackgroundImageInput input, AuthorInput authorInput, CancellationToken cancellationToken);
}
