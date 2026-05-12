using Microsoft.EntityFrameworkCore;
using ChallengeAPI.Data;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

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

app.Run();
