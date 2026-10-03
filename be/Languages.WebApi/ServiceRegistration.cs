using FluentValidation;
using Languages.Application.Persistence;
using Languages.Infrastructure.Persistence;
using Languages.WebApi.Filters;

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
        
        // Extras
        services.AddOpenApi();
        
        return services;
    }
}