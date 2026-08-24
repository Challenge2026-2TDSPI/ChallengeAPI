using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ChallengeAPI.Data;
using Scalar.AspNetCore;
using System.Reflection;
using Serilog;

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

builder.Services.AddControllers();

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

//app.UseHttpsRedirection(); comentei isso para não ter erro de HTTP para HTTPS, assim ficando fácil usar o swagger e scalar

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
