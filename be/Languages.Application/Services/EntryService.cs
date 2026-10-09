using Languages.Application.Extensions;
using Languages.Application.Interfaces;
using Languages.Application.Models.Common;
using Languages.Application.Models.Inputs.Dictionary;
using Languages.Application.Models.Inputs.Entry;
using Languages.Application.Models.Outputs;
using Languages.Application.Persistence;
using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Languages.Application.Services;

public class EntryService(
    ILanguagesDbContext dbContext,
    IInputValidation validation) : IEntryService
{
    public async Task<Result<GetManyEntriesOutput>> GetMany(GetManyEntriesInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<GetManyEntriesOutput>();
        
        if(!(await VerifyScope(input, cancellationToken)))
            return Result.Failure<GetManyEntriesOutput>("Dictionary not found.", ResultErrorKind.ResourceNotFound);

        var query = dbContext.Entries
            .AsNoTracking()
            .Include(e => e.Dictionary)
            .Where(e => e.DictionaryId == input.DictionaryId && e.Dictionary.LanguageId == input.LanguageId &&
                        (input.Type == null || input.Type == e.Type) && 
                        (input.Pagination.Search == null || e.Text.Contains(input.Pagination.Search)))
            .OrderBy(l => l.Id);
        
        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .Skip((input.Pagination.Page - 1) * input.Pagination.PageSize)
            .Take(input.Pagination.PageSize)
            .ToListAsync(cancellationToken);

        return Result.Success<GetManyEntriesOutput>("Entries were retrieved successfully.", new GetManyEntriesOutput(
            Items: result.Select(e => new GetOneEntryOutput(
                Id: e.Id,
                Text: e.Text,
                Translation: e.Translation,
                Type: e.Type,
                PhoneticTranscription: e.PhoneticTranscription,
                Dictionary: new DictionarySummaryOutput(
                    Id: e.DictionaryId,
                    Name: e.Dictionary.Name    
                ),
                CreatedAtUTC: e.CreatedAtUTC,
                LastUpdatedAtUTC: e.ModifiedAtUTC
            )).ToList(),
            Page: input.Pagination.Page,
            PageSize: input.Pagination.PageSize,
            TotalCount: totalCount
        ));
    }

    public async Task<Result<GetOneEntryOutput>> GetOne(GetOneEntryInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<GetOneEntryOutput>();

        var entry = await dbContext.Entries
            .AsNoTracking()
            .Include(e => e.Dictionary)
            .FirstOrDefaultAsync(e => e.DictionaryId == input.DictionaryId && e.Dictionary.LanguageId == input.LanguageId && e.Id == input.EntryId, cancellationToken);
        
        if (entry == null)
            return Result.Failure<GetOneEntryOutput>("Entry not found.", ResultErrorKind.ResourceNotFound);

        return Result.Success<GetOneEntryOutput>("Entry was retrieved successfully.", new GetOneEntryOutput(
            Id: entry.Id,
            Text: entry.Text,
            Translation: entry.Translation,
            Type: entry.Type,
            PhoneticTranscription: entry.PhoneticTranscription,
            Dictionary: new DictionarySummaryOutput(
                Id: entry.DictionaryId,
                Name: entry.Dictionary.Name    
            ),
            CreatedAtUTC: entry.CreatedAtUTC,
            LastUpdatedAtUTC: entry.ModifiedAtUTC
        ));
    }

    public async Task<Result<CreateEntryOutput>> Create(CreateEntryInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<CreateEntryOutput>();
        
        if(!(await VerifyScope(input, cancellationToken)))
            return Result.Failure<CreateEntryOutput?>("Dictionary not found.", ResultErrorKind.ResourceNotFound);
        
        var dictionary = await dbContext.Dictionaries.FirstOrDefaultAsync(d => d.Id == input.DictionaryId, cancellationToken);
        if (dictionary == null)
        {
            throw new InvalidDataException("Dictionary not found.");
        }

        var newEntry = new Entry
        {
            Text = input.Text,
            Translation = input.Translation,
            Type = input.Type,
            PhoneticTranscription = input.PhoneticTranscription,
            DictionaryId = dictionary.Id,
        };
        
        dbContext.Entries.Add(newEntry);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success<CreateEntryOutput>("Entry was created successfully", new CreateEntryOutput(
            Id: newEntry.Id,
            Text: newEntry.Text,
            Translation: newEntry.Translation,
            Type: newEntry.Type,
            PhoneticTranscription: newEntry.PhoneticTranscription,
            Dictionary: new DictionarySummaryOutput(
                Id: dictionary.Id,
                Name: dictionary.Name    
            ),
            CreatedAtUTC: newEntry.CreatedAtUTC
        ));
    }

    public async Task<Result<UpdateEntryOutput>> Update(UpdateEntryInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<UpdateEntryOutput>();

        var entry = await dbContext.Entries
            .Include(e => e.Dictionary)
            .FirstOrDefaultAsync(e => e.DictionaryId == input.DictionaryId && e.Dictionary.LanguageId == input.LanguageId && e.Id == input.EntryId, cancellationToken);

        if (entry == null)
            return Result.Failure<UpdateEntryOutput>("Entry not found.", ResultErrorKind.ResourceNotFound);

        if (input.Text.IsProvided)
        {
            entry.Text = input.Text.Value ?? throw new InvalidDataException("Entry text was null.");
        }
        
        if (input.Translation.IsProvided)
        {
            entry.Translation = input.Translation.Value ?? throw new InvalidDataException("Entry translation was null.");
        }
        
        if (input.Type.IsProvided)
        {
            entry.Type = input.Type.Value ?? throw new InvalidDataException("Entry type was null.");
        }
        
        if (input.PhoneticTranscription.IsProvided)
        {
            entry.PhoneticTranscription = input.PhoneticTranscription.Value;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success<UpdateEntryOutput>("Entry was updated successfully", new UpdateEntryOutput(
            Id: entry.Id,
            Text: entry.Text,
            Translation: entry.Translation,
            Type: entry.Type,
            PhoneticTranscription: entry.PhoneticTranscription,
            Dictionary: new DictionarySummaryOutput(
                Id: entry.Dictionary.Id,
                Name: entry.Dictionary.Name    
            ),
            LastUpdatedAtUTC: entry.ModifiedAtUTC
        ));
    }

    public async Task<Result> Delete(DeleteEntryInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult();
        
        var entry = await dbContext.Entries
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.DictionaryId == input.DictionaryId && e.Dictionary.LanguageId == input.LanguageId && e.Id == input.EntryId, cancellationToken);

        if (entry == null)
            return Result.Failure("Entry not found.", ResultErrorKind.ResourceNotFound);

        dbContext.Entries.Remove(entry);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success("Entry was deleted successfully.");
    }
    
    private async Task<bool> VerifyScope(BaseEntryInput input, CancellationToken cancellationToken) =>
        await dbContext.Dictionaries.AnyAsync(d => d.Id == input.DictionaryId && d.LanguageId == input.LanguageId, cancellationToken);
}
