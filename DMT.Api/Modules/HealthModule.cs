using Microsoft.Extensions.Diagnostics.HealthChecks;

public class HealthModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/health")
                        .WithTags("Health");

        group.MapGet("/", async (HealthCheckService healthCheckService) =>
        {
            var report = await healthCheckService.CheckHealthAsync();
            var response = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(x => new
                {
                    name = x.Key,
                    status = x.Value.Status.ToString(),
                    exception = x.Value.Exception?.Message,
                    duration = x.Value.Duration.ToString()
                }),
                duration = report.TotalDuration
            };

            return report.Status == HealthStatus.Healthy
                ? Results.Ok(response)
                : Results.Json(response, statusCode: 503);
        })
        .WithName("GetHealth")
        .WithSummary("Get application health status")
        .WithDescription("Returns the health status of the application and its dependencies")
        .Produces(200)
        .Produces(503);

        group.MapGet("/ready", async (HealthCheckService healthCheckService) =>
        {
            var report = await healthCheckService.CheckHealthAsync(check => check.Tags.Contains("ready"));
            return report.Status == HealthStatus.Healthy
                ? Results.Ok(new { status = "Ready" })
                : Results.Json(new { status = "Not Ready" }, statusCode: 503);
        })
        .WithName("GetReadiness")
        .WithSummary("Get readiness probe")
        .WithDescription("Returns 200 when the application is ready to serve requests")
        .Produces(200)
        .Produces(503);

        group.MapGet("/live", () =>
        {
            return Results.Ok(new { status = "Alive" });
        })
        .WithName("GetLiveness")
        .WithSummary("Get liveness probe")
        .WithDescription("Returns 200 when the application is alive")
        .Produces(200);
    }
}
