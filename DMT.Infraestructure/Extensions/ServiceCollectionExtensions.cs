using DMT.Application.Interfaces.Repositories;
using DMT.Application.Interfaces.Services;
using DMT.Application.Services;
using DMT.Infrastructure.Data;
using DMT.Infrastructure.Repositories;
using DMT.Infraestructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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

        // Register Redis caching
        var redisConnectionString = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            services.AddSingleton<IConnectionMultiplexer>(provider =>
                ConnectionMultiplexer.Connect(redisConnectionString));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "DMT";
            });

            services.AddScoped<ICacheService, RedisCacheService>();
        }

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<IProductService, ProductsService>();

        return services;
    }
}
