using RaceBoard.Common;
using RaceBoard.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment environment = builder.Environment;

var startup = new Startup(builder.Configuration, environment);

startup.ConfigureServices(builder.Services);

ConfigureLogging(builder, builder.Configuration);

builder.Host.UseDefaultServiceProvider(o =>
{
    o.ValidateOnBuild = true;
    o.ValidateScopes = true;
});

var app = builder.Build();

startup.Configure(app, builder.Environment);

app.UseSerilogRequestLogging();

app.Run();


void ConfigureLogging(WebApplicationBuilder builder, IConfiguration configuration)
{
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.WithCorrelationIdHeader(CommonValues.HttpCustomHeaders.CorrelationId)
        .CreateLogger();

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);
    builder.Host.UseSerilog(logger);
}