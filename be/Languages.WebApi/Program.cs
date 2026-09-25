using Languages.Infrastructure.Persistence;
using Languages.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
