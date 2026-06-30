namespace Zodiaco.Api.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<LeadsService>();
        services.AddScoped<QuoteRequestsService>();
        services.AddScoped<TrucksService>();

        return services;
    }
}
