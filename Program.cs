/*
.NET uses a configuration hierarchy that merges multiple appsettings
  files:
  1. appsettings.json - Base configuration for all environments
  2. appsettings.{Environment}.json - Environment-specific overrides
*/
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
