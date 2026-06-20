using Npgsql;

namespace Zodiaco.Api.Extensions;

public static class ConfigurationExtensions
{
    public static string GetDatabaseConnectionString(this IConfiguration configuration)
    {
        var environmentConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (!string.IsNullOrWhiteSpace(environmentConnectionString))
        {
            return environmentConnectionString;
        }

        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        if (!string.IsNullOrWhiteSpace(databaseUrl))
        {
            return BuildConnectionStringFromDatabaseUrl(databaseUrl);
        }

        var configuredConnectionString = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrWhiteSpace(configuredConnectionString))
        {
            return configuredConnectionString;
        }

        throw new InvalidOperationException(
            "No database connection string was configured. Set ConnectionStrings:DefaultConnection or DATABASE_URL.");
    }

    public static string[] GetAllowedCorsOrigins(this IConfiguration configuration)
    {
        var configuredOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        var environmentOrigins = configuration["CORS_ALLOWED_ORIGINS"]?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            ?? Array.Empty<string>();

        return configuredOrigins
            .Concat(environmentOrigins)
            .Where(origin => !string.IsNullOrWhiteSpace(origin))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static string BuildConnectionStringFromDatabaseUrl(string databaseUrl)
    {
        if (!Uri.TryCreate(databaseUrl, UriKind.Absolute, out var uri))
        {
            throw new InvalidOperationException("DATABASE_URL is not a valid URI.");
        }

        if (!uri.Scheme.Equals("postgres", StringComparison.OrdinalIgnoreCase)
            && !uri.Scheme.Equals("postgresql", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("DATABASE_URL must use the postgres or postgresql scheme.");
        }

        var credentials = uri.UserInfo.Split(':', 2, StringSplitOptions.None);
        var isLocalHost = IsLocalHost(uri.Host);
        var databaseName = uri.AbsolutePath.Trim('/');

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new InvalidOperationException("DATABASE_URL does not contain a database name.");
        }

        // Local PostgreSQL typically runs without SSL, while Railway and other hosted databases require it.
        var defaultSslMode = isLocalHost ? SslMode.Disable : SslMode.Require;
        var trustServerCertificateWasSpecified = false;

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort ? 5432 : uri.Port,
            Database = databaseName,
            Username = credentials.Length > 0 ? Uri.UnescapeDataString(credentials[0]) : string.Empty,
            Password = credentials.Length > 1 ? Uri.UnescapeDataString(credentials[1]) : string.Empty,
            SslMode = defaultSslMode
        };

        foreach (var queryParameter in uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = queryParameter.Split('=', 2, StringSplitOptions.None);
            var key = Uri.UnescapeDataString(parts[0]).ToLowerInvariant();
            var value = parts.Length > 1 ? Uri.UnescapeDataString(parts[1]) : string.Empty;

            switch (key)
            {
                case "sslmode":
                    if (Enum.TryParse<SslMode>(value, true, out var sslMode))
                    {
                        builder.SslMode = sslMode;
                    }
                    break;
                case "trustservercertificate":
                case "trust_server_certificate":
                    builder["Trust Server Certificate"] = value;
                    trustServerCertificateWasSpecified = true;
                    break;
            }
        }

        if (!isLocalHost && builder.SslMode != SslMode.Disable && !trustServerCertificateWasSpecified)
        {
            builder["Trust Server Certificate"] = "true";
        }

        return builder.ConnectionString;
    }

    public static bool IsLocalHost(string? host)
    {
        if (string.IsNullOrWhiteSpace(host))
        {
            return false;
        }

        return host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
               || host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase)
               || host.Equals("::1", StringComparison.OrdinalIgnoreCase)
               || host.Equals("[::1]", StringComparison.OrdinalIgnoreCase);
    }
}
