using Azure.Storage.Blobs;
using FluentValidation;
using Languages.Application.Interfaces;
using Languages.Infrastructure.Authentication;
using Languages.Infrastructure.Options;
using Languages.Infrastructure.Persistence;
using Languages.Infrastructure.Persistence.Interceptors;
using Languages.Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
            .Bind(configuration.GetSection(AzureBlobStorageOptions.AppSettingsSectionName))
                .Validate(o => !string.IsNullOrWhiteSpace(o.ContainerName), "ContainerName must be a non-empty string.")
            .ValidateOnStart();
        
        services.AddOptions<AuthOptions>()
            .Bind(configuration.GetSection(AuthOptions.AppSettingsSectionName))
                .Validate(o => o.JwtAccessTokenLifetimeMinutes > 0, "JwtAccessTokenLifetimeMinutes must be a positive integer.")
                .Validate(o => o.JwtRefreshTokenLifetimeDays > 0, "JwtRefreshTokenLifetimeDays must be a positive integer.")
                .Validate(o => !string.IsNullOrWhiteSpace(o.JwtSigningKey), "JwtSigningKey must be a non-empty string.")
                .Validate(o => !string.IsNullOrWhiteSpace(o.JwtIssuer), "JwtIssuer must be a non-empty string.")
                .Validate(o => !string.IsNullOrWhiteSpace(o.JwtAudience), "JwtAudience must be a non-empty string.")
            .ValidateOnStart();
        
        // Authentication
        services.AddSingleton<PasswordHasher<object>>();
        services.AddSingleton<IPasswordHashingService, PasswordHashingService>();
        services.AddSingleton<IAuthTokenService, AuthTokenService>();
        
        // Application Input DTOs validators
        services.AddScoped<IInputValidation, InputValidation>();
        foreach (var validator in AssemblyScanner.FindValidatorsInAssembly(typeof(IInputValidation).Assembly))
        {
            services.AddScoped(validator.InterfaceType, validator.ValidatorType);
        }

        // DbContext
        services.AddDbContext<LanguagesDbContext>((serviceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString("LanguagesDatabase") 
                                   ?? throw new InvalidOperationException("LanguagesDatabase connection string not set.");
            options.UseSqlServer(connectionString, (builder) =>
            {
                builder.EnableRetryOnFailure();
            });
            options.AddInterceptors(serviceProvider.GetRequiredService<EntityAuditInterceptor>());
        });

        // DbContext SaveChanges() interceptor
        services.AddScoped<EntityAuditInterceptor>();
        
        // Azure Blob Storage
        services.AddSingleton<BlobContainerClient>((serviceProvider) =>
        {
            var azureBlobStorageOptions = serviceProvider.GetRequiredService<IOptions<AzureBlobStorageOptions>>().Value;
            
            var azureBlobServiceClient = new BlobServiceClient(configuration.GetConnectionString("LanguagesBlobStorage")) 
                                         ?? throw new InvalidOperationException("LanguagesDatabase connection string not set.");
            
            return azureBlobServiceClient.GetBlobContainerClient(azureBlobStorageOptions.ContainerName);
        });

        return services;
    }
}
