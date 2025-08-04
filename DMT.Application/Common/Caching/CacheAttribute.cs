namespace DMT.Application.Common.Caching;

[AttributeUsage(AttributeTargets.Class)]
public class CacheAttribute : Attribute
{
    public TimeSpan? Duration { get; }
    public string? KeyPattern { get; }
    public bool InvalidateOnChange { get; }

    public CacheAttribute(int durationMinutes = 30, string? keyPattern = null, bool invalidateOnChange = false)
    {
        Duration = TimeSpan.FromMinutes(durationMinutes);
        KeyPattern = keyPattern;
        InvalidateOnChange = invalidateOnChange;
    }
}
