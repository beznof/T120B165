using Languages.Application.Extensions;
using Languages.Application.Interfaces;
using Languages.Application.Models.Common;
using Languages.Application.Models.Inputs.Common;
using Languages.Application.Models.Inputs.Dictionary;
using Languages.Application.Models.Outputs;
using Languages.Application.Persistence;
using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Languages.Application.Services;

public class DictionaryService(
    ILanguagesDbContext dbContext,
    IImageStorage imageStorage,
    IInputValidation validation) : IDictionaryService
{
    public async Task<Result<GetManyDictionariesOutput>> GetMany(GetManyDictionariesInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<GetManyDictionariesOutput>();
        
        if(!(await VerifyScope(input, cancellationToken)))
            return Result.Failure<GetManyDictionariesOutput>("Language not found.", ResultErrorKind.ResourceNotFound);

        var query = dbContext.Dictionaries
            .AsNoTracking()
            .Where(d => d.LanguageId == input.LanguageId && 
                        (input.Pagination.Search == null || d.Name.Contains(input.Pagination.Search)))
            .OrderBy(d => d.Id);
        
        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .Skip((input.Pagination.Page - 1) * input.Pagination.PageSize)
            .Take(input.Pagination.PageSize)
            .Select(d => new GetOneDictionaryOutput(
                Id: d.Id,
                Name: d.Name,
                BackgroundImageUrl: d.BackgroundImageUrl,
                EntriesCount: d.Entries.Count,
                Language: new LanguageSummaryOutput(
                    Id: d.LanguageId,
                    Name: d.Language.Name    
                ),
                Owner: new OwnerSummaryOutput(
                    Id: d.Owner.Id,
                    Username: d.Owner.Username
                ),
                CreatedAtUTC: d.CreatedAtUTC,
                LastUpdatedAtUTC: d.ModifiedAtUTC
            ))
            .ToListAsync(cancellationToken);

        return Result.Success<GetManyDictionariesOutput>("Dictionaries were retrieved successfully.", new GetManyDictionariesOutput(
            Items: result,
            Page: input.Pagination.Page,
            PageSize: input.Pagination.PageSize,
            TotalCount: totalCount
        ));
    }

    public async Task<Result<GetOneDictionaryOutput>> GetOne(GetOneDictionaryInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<GetOneDictionaryOutput>();

        var dictionary = await dbContext.Dictionaries
            .AsNoTracking()
            .Where(d => d.LanguageId == input.LanguageId && d.Id == input.DictionaryId)
            .Select(d => new GetOneDictionaryOutput(
                Id: d.Id,
                Name: d.Name,
                BackgroundImageUrl: d.BackgroundImageUrl,
                EntriesCount: d.Entries.Count,
                Language: new LanguageSummaryOutput(
                    Id: d.LanguageId,
                    Name: d.Language.Name 
                ),
                Owner: new OwnerSummaryOutput(
                    Id: d.Owner.Id,
                    Username: d.Owner.Username
                ),
                CreatedAtUTC: d.CreatedAtUTC,
                LastUpdatedAtUTC: d.ModifiedAtUTC
            ))
            .FirstOrDefaultAsync(cancellationToken);
        
        if (dictionary == null)
            return Result.Failure<GetOneDictionaryOutput>("Dictionary not found.", ResultErrorKind.ResourceNotFound);

        return Result.Success<GetOneDictionaryOutput>("Dictionary was retrieved successfully.", dictionary);
    }

    public async Task<Result<CreateDictionaryOutput>> Create(CreateDictionaryInput input, AuthorInput authorInput, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<CreateDictionaryOutput>();
        
        if(!(await VerifyScope(input, cancellationToken)))
            return Result.Failure<CreateDictionaryOutput>("Language not found.", ResultErrorKind.ResourceNotFound);
        
        var language = await dbContext.Languages.FirstOrDefaultAsync(l => l.Id == input.LanguageId, cancellationToken);
        if (language == null)
        {
            throw new InvalidDataException("Language not found.");
        }

        var newDictionary = new Dictionary
        {
            Name = input.Name.Trim(),
            LanguageId = language.Id,
            OwnerId = authorInput.AuthorId
        };
        
        dbContext.Dictionaries.Add(newDictionary);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == authorInput.AuthorId, cancellationToken);

        if (user == null)
            throw new InvalidDataException("User was not found.");

        return Result.Success<CreateDictionaryOutput>("Dictionary was created successfully", new CreateDictionaryOutput(
            Id: newDictionary.Id,
            Name: newDictionary.Name,
            BackgroundImageUrl: newDictionary.BackgroundImageUrl,
            Language: new LanguageSummaryOutput(
                Id: language.Id,
                Name: language.Name 
            ),
            Owner: new OwnerSummaryOutput(
                Id: user.Id,
                Username: user.Username
            ),
            CreatedAtUTC: newDictionary.CreatedAtUTC
        ));
    }

    public async Task<Result<UpdateDictionaryOutput>> Update(UpdateDictionaryInput input, AuthorInput authorInput, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<UpdateDictionaryOutput>();

        var dictionary = await dbContext.Dictionaries
            .Include(d => d.Language)
            .Include(d => d.Owner)
            .Where(d => d.LanguageId == input.LanguageId && d.Id == input.DictionaryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (dictionary == null)
            return Result.Failure<UpdateDictionaryOutput>("Dictionary not found.", ResultErrorKind.ResourceNotFound);
        
        if (dictionary.OwnerId != authorInput.AuthorId && !authorInput.IsAdmin)
            return Result.Failure<UpdateDictionaryOutput>("Action not allowed for this user.", ResultErrorKind.AuthorizationFailed);

        if (input.Name.IsProvided)
        {
            dictionary.Name = input.Name.Value?.Trim() ?? throw new InvalidDataException("Dictionary name was null.");
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Success<UpdateDictionaryOutput>("Dictionary was updated successfully", new UpdateDictionaryOutput(
            Id: dictionary.Id,
            Name: dictionary.Name,
            BackgroundImageUrl: dictionary.BackgroundImageUrl,
            Language: new LanguageSummaryOutput(
                Id: dictionary.Language.Id,
                Name: dictionary.Language.Name 
            ),
            Owner: new OwnerSummaryOutput(
                Id: dictionary.Owner.Id,
                Username: dictionary.Owner.Username
            ),
            LastUpdatedAtUTC: dictionary.ModifiedAtUTC
        ));
    }

    public async Task<Result> Delete(DeleteDictionaryInput input, AuthorInput authorInput, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult();

        var dictionary = await dbContext.Dictionaries
            .Where(d => d.LanguageId == input.LanguageId && d.Id == input.DictionaryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (dictionary == null)
            return Result.Failure("Dictionary not found.", ResultErrorKind.ResourceNotFound);
        
        if (dictionary.OwnerId != authorInput.AuthorId && !authorInput.IsAdmin)
            return Result.Failure("Action not allowed for this user.", ResultErrorKind.AuthorizationFailed);
        
        dbContext.Dictionaries.Remove(dictionary);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        if(!string.IsNullOrWhiteSpace(dictionary.BackgroundImageUrl))
            await imageStorage.DeleteImageAsync(dictionary.BackgroundImageUrl, cancellationToken);

        return Result.Success("Dictionary was deleted successfully.");
    }

    public async Task<Result<SetDictionaryBackgroundImageOutput>> SetBackgroundImage(SetDictionaryBackgroundImageInput input, AuthorInput authorInput, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<SetDictionaryBackgroundImageOutput>();
        
        var dictionary = await dbContext.Dictionaries
            .Where(d => d.LanguageId == input.LanguageId && d.Id == input.DictionaryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (dictionary == null)
            return Result.Failure<SetDictionaryBackgroundImageOutput>("Dictionary not found.", ResultErrorKind.ResourceNotFound);
        
        if (dictionary.OwnerId != authorInput.AuthorId && !authorInput.IsAdmin)
            return Result.Failure<SetDictionaryBackgroundImageOutput>("Action not allowed for this user.", ResultErrorKind.AuthorizationFailed);

        var image = input.Image;

        var extension = image.ContentType.ToLower() switch
        {
            "image/png" => ".png",
            "image/jpeg" => ".jpg",
            _ => throw new InvalidDataException("Invalid image type.")
        };

        var location = $"languages/{input.LanguageId}/dictionaries/{Guid.NewGuid()}{extension}";
        
        var tempBackgroundImageUrl = dictionary.BackgroundImageUrl;
        var imageUrl = await imageStorage.UploadImageAsync(location, input.Image, cancellationToken);

        dictionary.BackgroundImageUrl = imageUrl;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        if(!string.IsNullOrWhiteSpace(tempBackgroundImageUrl))
            await imageStorage.DeleteImageAsync(tempBackgroundImageUrl, cancellationToken);
        
        return Result.Success<SetDictionaryBackgroundImageOutput>("Image uploaded successfully.", new SetDictionaryBackgroundImageOutput(
            ImageUrl: imageUrl    
        ));
    }

    public async Task<Result> DeleteBackgroundImage(DeleteDictionaryBackgroundImageInput input, AuthorInput authorInput, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult();

        var dictionary = await dbContext.Dictionaries
            .Where(d => d.LanguageId == input.LanguageId && d.Id == input.DictionaryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (dictionary == null)
            return Result.Failure("Dictionary not found.", ResultErrorKind.ResourceNotFound);
        
        if (dictionary.OwnerId != authorInput.AuthorId && !authorInput.IsAdmin)
            return Result.Failure("Action not allowed for this user.", ResultErrorKind.AuthorizationFailed);
        
        if (string.IsNullOrWhiteSpace(dictionary.BackgroundImageUrl))
            return Result.Failure("Dictionary background image was not found.", ResultErrorKind.ResourceNotFound);
        
        var tempBackgroundImageUrl = dictionary.BackgroundImageUrl;
        dictionary.BackgroundImageUrl = null;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await imageStorage.DeleteImageAsync(tempBackgroundImageUrl, cancellationToken);

        return Result.Success("Dictionary background image was deleted successfully.");
    }

    private async Task<bool> VerifyScope(BaseDictionaryInput input, CancellationToken cancellationToken) =>
        await dbContext.Languages.AnyAsync(l => l.Id == input.LanguageId, cancellationToken);
}
