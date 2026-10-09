using System.Text.Json;
using Languages.Application.Interfaces;
using Languages.Application.Models.Common;
using Languages.Application.Persistence;
using Languages.Application.Services;
using Languages.Infrastructure.ImageStorage;
using Languages.Infrastructure.Persistence;
using Languages.WebApi.Extensions;
using Languages.WebApi.Filters;
using Languages.WebApi.ModelBinding;
using Languages.WebApi.Serialization;

namespace Languages.WebApi;

internal static class ServiceRegistration
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        // Controllers configuration
        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();

            // Custom binding error messages
            options.ModelMetadataDetailsProviders.Add(new ApiBindingMetadataProvider());
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            // Custom binding error response formatting
            options.InvalidModelStateResponseFactory = (context) =>
            {
                var isJsonDeserializationExceptionPresent = context.ModelState.Values
                    .SelectMany(e => e.Errors)
                    .Select(e => e.Exception)
                    .OfType<JsonException>()
                    .Any();

                var invalidParamModelStateMessage = context.ModelState
                    .Where(p => p.Value != null)
                    .SelectMany(p => p.Value!.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault(errorMessage => !string.IsNullOrWhiteSpace(errorMessage));

                string message;

                if (isJsonDeserializationExceptionPresent)
                {
                    message = "Invalid JSON request body.";
                }
                else if (invalidParamModelStateMessage != null)
                {
                    message = invalidParamModelStateMessage;
                }
                else
                {
                    message = "Invalid request.";
                }

                return Result.Failure(message, ResultErrorKind.ValidationFailed).ToActionResult();
            };
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.AllowDuplicateProperties = false;
            options.JsonSerializerOptions.Converters.Add(new PatchFieldJsonConverterFactory());
            options.AllowInputFormatterExceptionMessages = false;
        });

        // DbContext
        services.AddScoped<ILanguagesDbContext, LanguagesDbContext>();

        // Services
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IDictionaryService, DictionaryService>();
        services.AddScoped<IEntryService, EntryService>();
        services.AddScoped<IImageStorage, AzureBlobImageStorage>();

        // Extras
        services.AddOpenApi();

        return services;
    }
}
