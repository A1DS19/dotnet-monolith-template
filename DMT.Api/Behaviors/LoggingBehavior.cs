using System.Diagnostics;
using System.Text.Json;
using MediatR;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger,
    IConfiguration configuration
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false // Compact JSON for performance
    };

    private readonly bool _logRequestData = configuration.GetValue<bool>("Logging:LogRequestData", true);
    private readonly bool _logResponseData = configuration.GetValue<bool>("Logging:LogResponseData", false);
    private readonly int _maxDataLength = configuration.GetValue<int>("Logging:MaxDataLength", 1000);

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var requestName = typeof(TRequest).Name;
        var responseName = typeof(TResponse).Name;

        // Always log basic info
        logger.LogInformation("[START] Handling {RequestName} → {ResponseName}", requestName, responseName);

        // Conditionally log detailed request data
        if (_logRequestData && logger.IsEnabled(LogLevel.Debug))
        {
            var requestJson = TruncateIfNeeded(JsonSerializer.Serialize(request, JsonOptions));
            logger.LogDebug("📥 Request: {RequestData}", requestJson);
        }

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeTaken = timer.Elapsed;

        // Always log completion with timing
        logger.LogInformation(
            "[END] Completed {RequestName} → {ResponseName} ⏱️ {ElapsedMs}ms",
            requestName,
            responseName,
            timeTaken.TotalMilliseconds
        );

        // Conditionally log detailed response data
        if (_logResponseData && logger.IsEnabled(LogLevel.Debug))
        {
            var responseJson = TruncateIfNeeded(JsonSerializer.Serialize(response, JsonOptions));
            logger.LogDebug("📤 Response: {ResponseData}", responseJson);
        }

        // Performance warning for slow requests
        if (timeTaken.TotalSeconds > 3)
        {
            logger.LogWarning(
                "⚠️ [PERFORMANCE] Slow request: {RequestName} took {ElapsedMs}ms",
                requestName,
                timeTaken.TotalMilliseconds
            );
        }

        return response;
    }

    private string TruncateIfNeeded(string data)
    {
        if (data.Length <= _maxDataLength) return data;
        return data[..(_maxDataLength - 3)] + "...";
    }
}
