using System.Text.RegularExpressions;

namespace PortfolioApp.Infrastructure.Extensions;

public static class StringExtensions
{
    public static bool TrimCaseInsensitiveEquals(this string? str, string? value)
    {
        return str != null && str.Trim().Equals(value?.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    public static bool CaseInsensitiveContains(this string? str, string? value)
    {
        return str != null && value != null && str.Contains(value, StringComparison.OrdinalIgnoreCase);
    }

    public static string ConvertLinebreaks(this string text)
    {
        return text.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
    }

    public static string RemoveLineBreaks(this string text)
    {
        return text.Replace("\n\r", "").Replace("\r", "").Replace("\n", "");
    }

    /// <summary>
    /// Replaces a string pattern, case insensitive
    /// </summary>
    /// <param name="str"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns>Replaced string</returns>
    public static string ReplaceCaseInsensitive(this string str, string from, string to)
    {
        return Regex.Replace(str, from, to, RegexOptions.IgnoreCase);
    }

    public static string ReplaceKeys(this string str, Dictionary<string, string> entries)
    {
        var result = str;
        foreach (var entry in entries)
        {
            if (entry.Key != null)
            {
                var value = entry.Value ?? "";
                result = result.ReplaceCaseInsensitive(entry.Key, value);
            }
        }
        return result;
    }

    public static string GetCorrectIconForFileExtension(this string str)
    {
        if (str == "jpg" | str == "jpeg")
        {
            return "jpg";
        }

        return str;
    }
}