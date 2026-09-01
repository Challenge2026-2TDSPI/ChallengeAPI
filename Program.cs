using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ChallengeAPI.Data;
using Scalar.AspNetCore;
using System.Reflection;
using System.Diagnostics;
using Serilog;
using Serilog.Context;
using ChallengeAPI.Telemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File(
            path: "logs/log-.txt",
            rollingInterval: RollingInterval.Day)
        .Enrich.FromLogContext();
});

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddSource("ChallengeAPI")
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter();
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddMeter(AppMetrics.Meter.Name)
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter();
    });

builder.Services.AddScoped<TelemetryActionFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.AddService<TelemetryActionFilter>();
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    options.IncludeXmlComments(
        Path.Combine(AppContext.BaseDirectory, xmlFilename)
    );
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(
        builder.Configuration.GetConnectionString("OracleConnection")
    ));

builder.Services.AddHealthChecks()
    .AddOracle(
        connectionString: builder.Configuration.GetConnectionString("OracleConnection"),
        name: "oracle-database",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "oracle" }
    );

var app = builder.Build();

app.Use(async (context, next) =>
{
    var activity = Activity.Current;

    using (LogContext.PushProperty("TraceId", activity?.TraceId.ToString()))
    using (LogContext.PushProperty("SpanId", activity?.SpanId.ToString()))
    {
        await next();
    }
});

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Challenge API")
        .WithTheme(ScalarTheme.DeepSpace)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

    options.OpenApiRoutePattern = "/swagger/v1/swagger.json";
});

// app.UseHttpsRedirection(); Mantido desabilitado para facilitar o uso local com HTTP.

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();

public partial class Program
{
}