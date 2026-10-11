using Languages.Application.Models.Common;
using Languages.Application.Models.Inputs.Common;
using Languages.Application.Models.Inputs.Entry;
using Languages.Application.Models.Outputs;

namespace Languages.Application.Interfaces;

public interface IEntryService
{
    public Task<Result<GetManyEntriesOutput>> GetMany(GetManyEntriesInput input, CancellationToken cancellationToken);
    public Task<Result<GetOneEntryOutput>> GetOne(GetOneEntryInput input, CancellationToken cancellationToken);
    public Task<Result<CreateEntryOutput>> Create(CreateEntryInput input, AuthorInput authorInput, CancellationToken cancellationToken);
    public Task<Result<UpdateEntryOutput>> Update(UpdateEntryInput input, AuthorInput authorInput, CancellationToken cancellationToken);
    public Task<Result> Delete(DeleteEntryInput input, AuthorInput authorInput, CancellationToken cancellationToken);
}
