using Languages.Application.Extensions;
using Languages.Application.Interfaces;
using Languages.Application.Models.Common;
using Languages.Application.Models.Inputs.Language;
using Languages.Application.Models.Outputs;
using Languages.Application.Persistence;
using Languages.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Languages.Application.Services;

public class LanguageService(
    ILanguagesDbContext dbContext,
    IImageStorage imageStorage,
    IInputValidation validation) : ILanguageService
{
    public async Task<Result<GetManyLanguagesOutput?>> GetMany(GetManyLanguagesInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<GetManyLanguagesOutput?>();

        var query = dbContext.Languages
            .AsNoTracking()
            .Where(l => input.Pagination.Search == null || l.Name.Contains(input.Pagination.Search))
            .OrderBy(l => l.Id);
        
        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .Skip((input.Pagination.Page - 1) * input.Pagination.PageSize)
            .Take(input.Pagination.PageSize)
            .ToListAsync(cancellationToken);

        return Result.Success<GetManyLanguagesOutput?>("Languages were retrieved successfully.", new GetManyLanguagesOutput(
            Items: result.Select(l => new GetOneLanguageOutput(
                Id: l.Id,
                Name: l.Name,
                BackgroundImageUrl: l.BackgroundImageUrl
            )).ToList(),
            Page: input.Pagination.Page,
            PageSize: input.Pagination.PageSize,
            TotalCount: totalCount
        ));
    }

    public async Task<Result<GetOneLanguageOutput?>> GetOne(GetOneLanguageInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<GetOneLanguageOutput?>();

        var language = await dbContext.Languages
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == input.LanguageId, cancellationToken);

        if (language == null)
            return Result.Failure<GetOneLanguageOutput?>("Language not found.", ResultErrorKind.ResourceNotFound);

        return Result.Success<GetOneLanguageOutput?>("Language was retrieved successfully.", new GetOneLanguageOutput(
            Id: language.Id,
            Name: language.Name,
            BackgroundImageUrl: language.BackgroundImageUrl
        ));
    }

    public async Task<Result<CreateLanguageOutput?>> Create(CreateLanguageInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<CreateLanguageOutput?>();

        var newLanguage = new Language
        {
            Name = input.Name
        };
        
        dbContext.Languages.Add(newLanguage);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success<CreateLanguageOutput?>("Language was created successfully", new CreateLanguageOutput(
            Id: newLanguage.Id,
            Name: newLanguage.Name,
            BackgroundImageUrl: newLanguage.BackgroundImageUrl
        ));
    }

    public async Task<Result> Update(UpdateLanguageInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult();
        
        var language = await dbContext.Languages
            .FirstOrDefaultAsync(l => l.Id == input.LanguageId, cancellationToken);

        if (language == null)
            return Result.Failure("Language not found.", ResultErrorKind.ResourceNotFound);

        if (input.Name.IsProvided)
        {
            language.Name = input.Name.Value ?? throw new InvalidDataException("Language name was null.");
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success("Language was updated successfully.");
    }

    public async Task<Result> Delete(DeleteLanguageInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult();
        
        var language = await dbContext.Languages
            .Include(l => l.Dictionaries)
            .FirstOrDefaultAsync(l => l.Id == input.LanguageId, cancellationToken);

        if (language == null)
            return Result.Failure("Language not found.", ResultErrorKind.ResourceNotFound);
        
        dbContext.Languages.Remove(language);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        if(!string.IsNullOrWhiteSpace(language.BackgroundImageUrl))
            await imageStorage.DeleteImageAsync(language.BackgroundImageUrl, cancellationToken);
        
        var dictionariesBackgroundImagesUrls = language.Dictionaries.Select(d => d.BackgroundImageUrl);
        foreach (var dictionariesBackgroundImageUrl in dictionariesBackgroundImagesUrls)
        {
            if(!string.IsNullOrWhiteSpace(dictionariesBackgroundImageUrl))
                await imageStorage.DeleteImageAsync(dictionariesBackgroundImageUrl, cancellationToken);
        }

        return Result.Success("Language was deleted successfully.");
    }

    public async Task<Result<SetLanguageBackgroundImageOutput?>> SetBackgroundImage(SetLanguageBackgroundImageInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult<SetLanguageBackgroundImageOutput?>();
        
        var language = await dbContext.Languages
            .FirstOrDefaultAsync(l => l.Id == input.LanguageId, cancellationToken);

        if (language == null)
            return Result.Failure<SetLanguageBackgroundImageOutput?>("Language not found.", ResultErrorKind.ResourceNotFound);

        var image = input.Image;

        var extension = image.ContentType.ToLower() switch
        {
            "image/png" => ".png",
            "image/jpeg" => ".jpg",
            _ => throw new InvalidDataException("Invalid image type.")
        };

        var location = $"languages/{input.LanguageId}/{Guid.NewGuid()}{extension}";
        
        var imageUrl = await imageStorage.UploadImageAsync(location, input.Image, cancellationToken);

        var tempBackgroundImageUrl = language.BackgroundImageUrl;
        language.BackgroundImageUrl = imageUrl;
        
        await dbContext.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(tempBackgroundImageUrl))
            await imageStorage.DeleteImageAsync(tempBackgroundImageUrl, cancellationToken);
        
        return Result.Success<SetLanguageBackgroundImageOutput?>("Image uploaded successfully.", new SetLanguageBackgroundImageOutput(
            ImageUrl: imageUrl    
        ));
        
    }

    public async Task<Result> DeleteBackgroundImage(DeleteLanguageBackgroundImageInput input, CancellationToken cancellationToken)
    {
        var inputValidation = await validation.ValidateAsync(input, cancellationToken);
        if (!inputValidation.IsValid)
            return inputValidation.ToFailureResult();
        
        var language = await dbContext.Languages
            .FirstOrDefaultAsync(l => l.Id == input.LanguageId, cancellationToken);

        if (language == null)
            return Result.Failure("Language not found.", ResultErrorKind.ResourceNotFound);
        
        if (string.IsNullOrWhiteSpace(language.BackgroundImageUrl))
            return Result.Failure("Language background image was not found.", ResultErrorKind.ResourceNotFound);

        var tempBackgroundImageUrl = language.BackgroundImageUrl;
        language.BackgroundImageUrl = null;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        await imageStorage.DeleteImageAsync(tempBackgroundImageUrl, cancellationToken);

        return Result.Success("Language background image was deleted successfully.");
    }
}
