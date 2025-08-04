using System.Reflection;

namespace DMT.Application;

/// <summary>
/// Marker class for Application assembly reference.
/// Used for assembly scanning in dependency injection.
/// </summary>
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}