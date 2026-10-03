using System.Text.Json;
using FluentValidation;
using Languages.Application.Interfaces;
using Languages.Application.Persistence;
using Languages.Application.Services;
using Languages.Infrastructure.Persistence;
using Languages.WebApi.Filters;
using Languages.WebApi.Serialization;

namespace Languages.WebApi;

internal static class ServiceRegistration
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        // Filters
        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
            options.Filters.Add<ValidationFilter>();
        });
        
        // FluentValidation
        foreach (var validator in AssemblyScanner.FindValidatorsInAssembly(typeof(ServiceRegistration).Assembly))
        {
            services.AddScoped(validator.InterfaceType, validator.ValidatorType);
        }
        
        // DbContext
        services.AddScoped<ILanguagesDbContext, LanguagesDbContext>();
        
        // Services
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<ILanguageDictionaryService, LanguageDictionaryService>();
        services.AddScoped<IDictionaryEntryService, DictionaryEntryService>();

        // JSON Serialization
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        jsonSerializerOptions.Converters.Add(new PatchFieldJsonConverterFactory());
        
        // Extras
        services.AddOpenApi();
        
        return services;
    }
}