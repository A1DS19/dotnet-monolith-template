/*
.NET uses a configuration hierarchy that merges multiple appsettings
  files:
  1. appsettings.json - Base configuration for all environments
  2. appsettings.{Environment}.json - Environment-specific overrides
*/

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
#region Services
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

builder.Services.AddOpenApi();
builder.Services.AddCarter();
#endregion Services

var app = builder.Build();
#region Middlewares
app.UseCors();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.MapCarter();
#endregion Middlewares

app.Run();
