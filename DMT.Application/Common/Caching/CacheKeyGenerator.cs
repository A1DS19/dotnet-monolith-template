using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DMT.Application.Common.Caching;

public static class CacheKeyGenerator
{
    public static string GenerateKey<T>(T request, string? pattern = null)
    {
        if (!string.IsNullOrEmpty(pattern) && request != null)
        {
            return InterpolatePattern(pattern, request);
        }

        var typeName = typeof(T).Name;
        var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var hash = ComputeHash(requestJson);
        return $"{typeName}:{hash}";
    }

    public static string GenerateKey(Type requestType, object request, string? pattern = null)
    {
        if (!string.IsNullOrEmpty(pattern))
        {
            return InterpolatePattern(pattern, request);
        }

        var typeName = requestType.Name;
        var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var hash = ComputeHash(requestJson);
        return $"{typeName}:{hash}";
    }

    private static string InterpolatePattern(string pattern, object request)
    {
        var result = pattern;
        var properties = request.GetType().GetProperties();

        foreach (var prop in properties)
        {
            var placeholder = $"{{{prop.Name}}}";
            var value = prop.GetValue(request)?.ToString() ?? "null";
            result = result.Replace(placeholder, value, StringComparison.OrdinalIgnoreCase);
        }

        return result;
    }

    private static string ComputeHash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash)[..16].ToLowerInvariant();
    }
}
