using Languages.Infrastructure.Persistence;
using Languages.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Languages.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // SaveChanges() interceptor
        services.AddScoped<EntityAuditInterceptor>();

        // DbContext
        services.AddDbContext<LanguagesDbContext>((serviceProvider, options) =>
        {
            var connectionString = "a";//configuration.GetConnectionString("LanguageDatabase") ?? throw new InvalidOperationException("LanguageDatabase connection string not set.");
            options.UseSqlServer(connectionString);
            options.AddInterceptors(serviceProvider.GetRequiredService<EntityAuditInterceptor>());
        });
        
        return services;
    }
}