using FluentValidation;
using Languages.Infrastructure.Persistence;
using Languages.Infrastructure.Persistence.Interceptors;
using Languages.WebApi.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<ApiResponseFilter>();
});

foreach (var validator in AssemblyScanner.FindValidatorsInAssembly(typeof(Program).Assembly))
{
    builder.Services.AddScoped(validator.InterfaceType, validator.ValidatorType);
}

builder.Services.AddOpenApi();

#region Database configuration

var connectionString = builder.Configuration.GetConnectionString("LanguageDatabase") ??
                       throw new InvalidOperationException("LanguageDatabase connection string not set.");

builder.Services.AddScoped<EntityAuditInterceptor>();

builder.Services.AddDbContext<LanguagesDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString);

    options.AddInterceptors(serviceProvider.GetRequiredService<EntityAuditInterceptor>());
});

#endregion

builder.Services.AddAutoMapper(cfg => {}, typeof(Program).Assembly);

builder.Services.AddScoped<ILanguagesDbContext>(provider => provider.GetRequiredService<LanguagesDbContext>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
