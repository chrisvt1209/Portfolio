namespace Portfolio.Infrastructure.Helpers;

public static class IconHelper
{
    private const string GitHubPath =
        "M12 1.5A10.5 10.5 0 0 0 8.68 21.96c.53.1.72-.23.72-.52v-1.82c-2.95.64-3.58-1.25-3.58-1.25-.48-1.2-1.17-1.52-1.17-1.52-.96-.65.07-.64.07-.64 1.06.08 1.62 1.09 1.62 1.09.95 1.62 2.48 1.15 3.08.88.1-.69.37-1.15.68-1.42-2.36-.27-4.84-1.18-4.84-5.25 0-1.16.42-2.12 1.09-2.87-.1-.27-.47-1.38.1-2.88 0 0 .9-.29 2.96 1.09a10.2 10.2 0 0 1 5.38 0c2.06-1.38 2.96-1.1 2.96-1.1.58 1.5.21 2.62.1 2.89.68.75 1.1 1.71 1.1 2.87 0 4.08-2.49 4.97-4.87 5.23.38.33.72.98.72 1.98v2.94c0 .29.19.63.73.52A10.5 10.5 0 0 0 12 1.5Z";

    private const string LinkedInPath =
        "M6.94 8.5H3.56V19.5h3.38V8.5ZM5.25 3A2.01 2.01 0 0 0 3.25 5c0 1.11.9 2 2 2s2-.89 2-2c0-1.1-.9-2-2-2Zm6.89 5.5H8.88V19.5h3.26v-5.44c0-1.43.27-2.82 2.04-2.82 1.74 0 1.76 1.63 1.76 2.91v5.35H19.2v-6.01c0-2.95-.64-5.22-4.08-5.22-1.65 0-2.75.91-3.2 1.77h-.05V8.5h.27Z";

    /// <summary>
    /// SVG path (24x24 viewBox) for a social network, or null when there is no bundled icon.
    /// </summary>
    public static string? GetSocialPath(string? network) => network?.ToLowerInvariant() switch
    {
        "github" => GitHubPath,
        "linkedin" => LinkedInPath,
        _ => null
    };

    /// <summary>
    /// Maps an Umbraco icon picker value (e.g. "icon-folder") to a Font Awesome class string.
    /// </summary>
    public static string GetFontAwesomeClass(string? umbracoIcon)
    {
        var name = (umbracoIcon ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;

        return name switch
        {
            "icon-folder" => "fa-regular fa-folder-open",
            "icon-game" => "fa-solid fa-gamepad",
            "icon-calendar" => "fa-regular fa-calendar",
            "" => "fa-regular fa-folder-open",
            _ => $"fa-solid fa-{name.Replace("icon-", string.Empty)}"
        };
    }
}
