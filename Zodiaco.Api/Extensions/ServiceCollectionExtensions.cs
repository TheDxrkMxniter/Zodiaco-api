using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Zodiaco.Api.Common;
using Zodiaco.Api.Data;

namespace Zodiaco.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetDatabaseConnectionString()));

        var allowedOrigins = configuration.GetAllowedCorsOrigins();
        services.AddCors(options =>
        {
            options.AddPolicy(ApplicationConstants.CorsPolicyName, policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = ApplicationConstants.ServiceName,
                Version = "v1",
                Description = "Backend base para Universal Zodiaco."
            });
        });

        return services;
    }
}
