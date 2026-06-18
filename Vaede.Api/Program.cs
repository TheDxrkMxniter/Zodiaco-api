using Microsoft.AspNetCore.HttpOverrides;
using Vaede.Api.Common;
using Vaede.Api.Extensions;
using Vaede.Api.Services;

DotNetEnv.Env.NoClobber().TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var railwayPort = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(railwayPort))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{railwayPort}");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddApplicationServices();
builder.Services.AddApplicationInfrastructure(builder.Configuration);

var app = builder.Build();

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
