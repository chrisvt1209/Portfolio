using System.Reflection;

namespace Portfolio.Services;

public class VersionService
{
    public static string GetVersion()
    {
        return string.Join(".",
            Assembly.GetExecutingAssembly().GetName().Version?.ToString().Split('.').Take(3) ??
            ArraySegment<string>.Empty);
    }
}
