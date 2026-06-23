using Microsoft.AspNetCore.HttpOverrides;
using Npgsql;
using Zodiaco.Api.Common;
using Zodiaco.Api.Data.Seed;
using Zodiaco.Api.Extensions;
using Zodiaco.Api.Services;
using ZodiacoConfigurationExtensions = Zodiaco.Api.Extensions.ConfigurationExtensions;

DotEnvLoader.LoadFromKnownLocations();

var builder = WebApplication.CreateBuilder(args);

var railwayPort = Environment.GetEnvironmentVariable("PORT");
var shouldSeed = args.Contains("--seed", StringComparer.OrdinalIgnoreCase);
if (!string.IsNullOrWhiteSpace(railwayPort))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{railwayPort}");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddScoped<DatabaseSeeder>();

builder.Services.AddApplicationServices();
builder.Services.AddApplicationInfrastructure(builder.Configuration);

var app = builder.Build();

if (shouldSeed)
{
    if (!app.Environment.IsDevelopment())
    {
        throw new InvalidOperationException("Seed is only allowed in Development.");
    }

    await using var scope = app.Services.CreateAsyncScope();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetDatabaseConnectionString();
    if (!ZodiacoConfigurationExtensions.IsLocalHost(new NpgsqlConnectionStringBuilder(connectionString).Host))
    {
        throw new InvalidOperationException("Seed is only allowed against a local PostgreSQL instance.");
    }

    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    var seedResult = await seeder.SeedAsync();
    Console.WriteLine(
        $"Seed completed. Inserted trucks: {seedResult.InsertedCount}. Updated trucks: {seedResult.UpdatedCount}");
    return;
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = $"{ApplicationConstants.ServiceName} Docs";
        options.RoutePrefix = string.Empty;
        options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{ApplicationConstants.ServiceName} v1");
    });
}

app.UseCors(ApplicationConstants.CorsPolicyName);
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
