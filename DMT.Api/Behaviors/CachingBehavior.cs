using DMT.Application.Common.Caching;
using DMT.Application.Common.CQRS;
using DMT.Application.Interfaces.Services;
using MediatR;
using System.Reflection;

namespace DMT.Api.Behaviors;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    public CachingBehavior(ICacheService cacheService, ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Only cache queries, not commands
        if (request is not IQuery<TResponse>)
        {
            var commandResponse = await next();
            await InvalidateCacheIfNeeded(request, cancellationToken);
            return commandResponse;
        }

        var cacheAttribute = typeof(TRequest).GetCustomAttribute<CacheAttribute>();
        if (cacheAttribute == null)
        {
            return await next();
        }

        var cacheKey = CacheKeyGenerator.GenerateKey(request, cacheAttribute.KeyPattern);

        // Try to get from cache
        var cachedResponse = await _cacheService.GetAsync<TResponse>(cacheKey, cancellationToken);
        if (cachedResponse != null)
        {
            _logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
            return cachedResponse;
        }

        // Cache miss - execute handler
        _logger.LogInformation("Cache miss for key: {CacheKey}", cacheKey);
        var response = await next();

        // Store in cache
        await _cacheService.SetAsync(cacheKey, response, cacheAttribute.Duration, cancellationToken);
        _logger.LogInformation("Cached response for key: {CacheKey} with expiration: {Duration}",
            cacheKey, cacheAttribute.Duration);

        return response;
    }

    private async Task InvalidateCacheIfNeeded(TRequest request, CancellationToken cancellationToken)
    {
        var invalidateAttribute = typeof(TRequest).GetCustomAttribute<InvalidateCacheAttribute>();
        if (invalidateAttribute == null) return;

        foreach (var pattern in invalidateAttribute.Patterns)
        {
            try
            {
                await _cacheService.RemoveByPatternAsync(pattern, cancellationToken);
                _logger.LogInformation("Invalidated cache with pattern: {Pattern}", pattern);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to invalidate cache with pattern: {Pattern}", pattern);
            }
        }
    }
}
