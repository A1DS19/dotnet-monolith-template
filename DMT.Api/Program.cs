/*
.NET uses a configuration hierarchy that merges multiple appsettings
  files:
  1. appsettings.json - Base configuration for all environments
  2. appsettings.{Environment}.json - Environment-specific overrides
*/

using Scalar.AspNetCore;
using Serilog;
using DMT.Infrastructure.Extensions;
using FluentValidation;
using DMT.Api.Exceptions;
using DMT.Api.Behaviors;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
#region Services
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        var allowedOrigins =
            builder.Configuration.GetSection("ApiSettings:AllowedOrigins")
                .Get<string[]>() ??
            [];
        configuration.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader =
            ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                     new QueryStringApiVersionReader("version"),
                                     new HeaderApiVersionReader("X-Version"));
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();

// Add FluentValidation from Application assembly
builder.Services.AddValidatorsFromAssembly(DMT.Application.AssemblyReference.Assembly);

// Add MediatR with assembly scanning
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(DMT.Application.AssemblyReference.Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
});

// Add exception handling
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

// Add Rate Limiting
var rateLimitConfig = builder.Configuration.GetSection("RateLimiting");
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = rateLimitConfig.GetValue<int>("FixedWindow:PermitLimit");
        limiterOptions.Window = TimeSpan.Parse(rateLimitConfig.GetValue<string>("FixedWindow:Window")!);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 5;
    });

    options.AddSlidingWindowLimiter("SlidingPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = rateLimitConfig.GetValue<int>("SlidingWindow:PermitLimit");
        limiterOptions.Window = TimeSpan.Parse(rateLimitConfig.GetValue<string>("SlidingWindow:Window")!);
        limiterOptions.SegmentsPerWindow = rateLimitConfig.GetValue<int>("SlidingWindow:SegmentsPerWindow");
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2;
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", token);
    };
});

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy())
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sqlserver",
        tags: new[] { "db", "sql", "sqlserver" })
    .AddRedis(
        builder.Configuration.GetConnectionString("Redis")!,
        name: "redis",
        tags: new[] { "cache", "redis" });

builder.Services.AddOpenApi();
builder.Services.AddCarter();
#endregion Services

var app = builder.Build();
#region Middlewares
app.UseExceptionHandler();
app.UseCors();
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.MapCarter();
#endregion Middlewares

try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
