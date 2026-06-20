using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Zodiaco.Api.Extensions;

namespace Zodiaco.Api.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        DotEnvLoader.LoadFromKnownLocations();

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var projectPath = ResolveProjectPath();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(projectPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetDatabaseConnectionString());

        return new AppDbContext(optionsBuilder.Options);
    }

    private static string ResolveProjectPath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var candidatePaths = new[]
        {
            currentDirectory,
            Path.Combine(currentDirectory, "Zodiaco.Api"),
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."))
        };

        foreach (var candidatePath in candidatePaths
                     .Select(Path.GetFullPath)
                     .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (File.Exists(Path.Combine(candidatePath, "appsettings.json")))
            {
                return candidatePath;
            }
        }

        throw new DirectoryNotFoundException(
            "Could not resolve the project directory for EF Core design-time configuration.");
    }
}
