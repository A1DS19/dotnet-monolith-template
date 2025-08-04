using DMT.Application.Interfaces.Repositories;
using DMT.Application.Interfaces.Services;
using DMT.Application.Services;
using DMT.Infrastructure.Data;
using DMT.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DMT.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Register database settings
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Database connection not found in configuration");

        // Register connection factory
        services.AddSingleton<IDbConnectionFactory>(_ =>
                new SqlServerConnectionFactory(connectionString));

        // Register repositories
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<IProductService, ProductsService>();

        return services;
    }
}
