using System.Text.Json;
using Languages.Application.Interfaces;
using Languages.Application.Persistence;
using Languages.Application.Services;
using Languages.Infrastructure.ImageStorage;
using Languages.Infrastructure.Persistence;
using Languages.WebApi.Filters;
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
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.AllowDuplicateProperties = false;
            options.JsonSerializerOptions.Converters.Add(new PatchFieldJsonConverterFactory());
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
