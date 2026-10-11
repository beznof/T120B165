using Languages.Infrastructure;
using Languages.WebApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddInfrastructure(configuration)
    .AddWebApi(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
