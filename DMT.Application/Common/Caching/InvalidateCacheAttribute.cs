namespace DMT.Application.Common.Caching;

[AttributeUsage(AttributeTargets.Class)]
public class InvalidateCacheAttribute : Attribute
{
    public string[] Patterns { get; }

    public InvalidateCacheAttribute(params string[] patterns)
    {
        Patterns = patterns ?? throw new ArgumentNullException(nameof(patterns));
    }
}
