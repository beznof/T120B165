using Languages.Infrastructure;
using Languages.WebApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddInfrastructure(configuration)
    .AddWebApi(configuration);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .WithOrigins("http://localhost:5173", "http://localhost:4173")
    .WithMethods("GET", "POST", "PATCH", "PUT", "DELETE")
    .WithHeaders("Authorization", "Content-Type")));

var app = builder.Build();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseCors();
    app.MapOpenApi();
    app.MapScalarApiReference("api/docs", (options) =>
    {
        options.WithTitle("Language Dictionaries API")
            .HideModels()
            .WithClassicLayout()
            .ForceDarkMode()
            .WithTheme(ScalarTheme.DeepSpace)
            .WithDefaultHttpClient(ScalarTarget.Http, ScalarClient.Http11)
            .WithYamlDocumentDownload()
            .HideDeveloperTools()
            .HideSearch()
            .SortTagsAlphabetically()
            .SortOperationsByMethod();
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
