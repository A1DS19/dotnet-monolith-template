/*
.NET uses a configuration hierarchy that merges multiple appsettings
  files:
  1. appsettings.json - Base configuration for all environments
  2. appsettings.{Environment}.json - Environment-specific overrides
*/
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

#endregion Services

var app = builder.Build();
#region Middlewares
app.UseCors();

app.MapGet("/", () => "Hello World!");

#endregion Middlewares

app.Run();
