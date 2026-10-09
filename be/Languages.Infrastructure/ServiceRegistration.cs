using Azure.Identity;
using Azure.Storage.Blobs;
using FluentValidation;
using Languages.Application.Interfaces;
using Languages.Infrastructure.ImageStorage;
using Languages.Infrastructure.Options;
using Languages.Infrastructure.Persistence;
using Languages.Infrastructure.Persistence.Interceptors;
using Languages.Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Languages.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration
        services.AddOptions<AzureBlobStorageOptions>()
            .Bind(configuration.GetSection(AzureBlobStorageOptions.AppSettingsSectionName));
        
        // Application Input DTOs validators
        services.AddScoped<IInputValidation, InputValidation>();
        foreach (var validator in AssemblyScanner.FindValidatorsInAssembly(typeof(IInputValidation).Assembly))
        {
            services.AddScoped(validator.InterfaceType, validator.ValidatorType);
        }

        // DbContext
        services.AddDbContext<LanguagesDbContext>((serviceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString("LanguagesDatabase") ?? throw new InvalidOperationException("LanguagesDatabase connection string not set.");
            options.UseSqlServer(connectionString);
            options.AddInterceptors(serviceProvider.GetRequiredService<EntityAuditInterceptor>());
        });

        // DbContext SaveChanges() interceptor
        services.AddScoped<EntityAuditInterceptor>();
        
        // Azure Blob Storage
        services.AddSingleton<BlobContainerClient>((serviceProvider) =>
        {
            var azureBlobStorageOptions = serviceProvider.GetRequiredService<IOptions<AzureBlobStorageOptions>>().Value;
            
            var azureBlobServiceClient = new BlobServiceClient(
                new Uri(azureBlobStorageOptions.ServiceUri),
                new DefaultAzureCredential()
            );
            
            return azureBlobServiceClient.GetBlobContainerClient(azureBlobStorageOptions.ContainerName);
        });

        return services;
    }
}
