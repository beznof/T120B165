using System.Text.Json;
using System.Text.Json.Serialization;
using Languages.Application.Interfaces;
using Languages.Application.Models.Common;
using Languages.Application.Persistence;
using Languages.Application.Services;
using Languages.Domain.Enums;
using Languages.Infrastructure.ImageStorage;
using Languages.Infrastructure.Options;
using Languages.Infrastructure.Persistence;
using Languages.WebApi.Extensions;
using Languages.WebApi.Filters;
using Languages.WebApi.ModelBinding;
using Languages.WebApi.OpenApi;
using Languages.WebApi.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<UserRole>());
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<DictionaryEntryType>());
            options.AllowInputFormatterExceptionMessages = false;
        });

        // DbContext
        services.AddScoped<ILanguagesDbContext, LanguagesDbContext>();

        // Services
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IDictionaryService, DictionaryService>();
        services.AddScoped<IEntryService, EntryService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IImageStorage, AzureBlobImageStorage>();

        // Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
        
        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<AuthOptions>>((options, authOptions) =>
            {
                var _authOptions = authOptions.Value;
                
                options.MapInboundClaims = false;
                options.IncludeErrorDetails = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Token signature
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_authOptions.JwtSigningKey)),
                    RequireSignedTokens = true,
                    // Token validation
                    ValidateIssuer = true,
                    ValidIssuer = _authOptions.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _authOptions.JwtAudience,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                    ClockSkew = TimeSpan.Zero,
                    // Claim mapping
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });
        
        services.AddAuthorization();

        // OpenAPI
        services.AddOpenApi(options =>
        {
            options.AddOperationTransformer(new InheritedParameterDocumentationTransformer());
            options.AddSchemaTransformer<OptionalSchemaTransformer>();
        });
        services.AddTransient<IApiDescriptionProvider, RequiredParameterMetadataProvider>();

        return services;
    }
}
