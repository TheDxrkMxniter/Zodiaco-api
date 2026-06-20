namespace Zodiaco.Api.Extensions;

public static class DotEnvLoader
{
    public static void LoadFromKnownLocations()
    {
        foreach (var candidatePath in GetCandidatePaths())
        {
            if (!File.Exists(candidatePath))
            {
                continue;
            }

            DotNetEnv.Env.NoClobber().Load(candidatePath);
            return;
        }
    }

    private static IEnumerable<string> GetCandidatePaths()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var baseDirectory = AppContext.BaseDirectory;

        return new[]
        {
            Path.Combine(currentDirectory, ".env"),
            Path.Combine(currentDirectory, "Zodiaco.Api", ".env"),
            Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", ".env")),
            Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", "..", ".env"))
        }
        .Select(Path.GetFullPath)
        .Distinct(StringComparer.OrdinalIgnoreCase);
    }
}
