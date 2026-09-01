using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChallengeAPI.Telemetry;

/// <summary>
/// Cria spans customizados e registra métricas para todas as ações dos controllers.
/// A instrumentação automática do ASP.NET Core continua responsável pelo trace HTTP.
/// </summary>
public sealed class TelemetryActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var controller = context.Controller.GetType().Name;
        var action = context.ActionDescriptor.DisplayName ?? context.ActionDescriptor.RouteValues["action"] ?? "Unknown";
        var method = context.HttpContext.Request.Method;

        using var activity = AppTelemetry.ActivitySource.StartActivity(
            $"API {method} {controller}.{action}");

        activity?.SetTag("http.request.method", method);
        activity?.SetTag("api.controller", controller);
        activity?.SetTag("api.action", action);

        var stopwatch = Stopwatch.StartNew();
        ActionExecutedContext? executedContext = null;

        try
        {
            executedContext = await next();

            var statusCode = executedContext.HttpContext.Response.StatusCode;
            activity?.SetTag("http.response.status_code", statusCode);

            if (statusCode >= 500)
            {
                activity?.SetStatus(ActivityStatusCode.Error);
            }
            else
            {
                activity?.SetStatus(ActivityStatusCode.Ok);
            }
        }
        catch (Exception ex)
        {
            activity?.SetTag("error.type", ex.GetType().FullName);
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
        finally
        {
            stopwatch.Stop();

            AppMetrics.RequestsCounter.Add(
                1,
                new TagList
                {
                    { "controller", controller },
                    { "action", action },
                    { "method", method }
                });

            AppMetrics.RequestDuration.Record(
                stopwatch.Elapsed.TotalMilliseconds,
                new TagList
                {
                    { "controller", controller },
                    { "action", action },
                    { "method", method }
                });
        }
    }
}
