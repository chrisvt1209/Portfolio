using System.Globalization;

namespace Portfolio.Infrastructure.Extensions;

public static class CurrencyTools
{
    private static readonly Dictionary<string, string?> Map;

    static CurrencyTools()
    {
        Map = CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .Where(c => !c.IsNeutralCulture)
            .Select(culture =>
            {
                try
                {
                    return new RegionInfo(culture.LCID);
                }
                catch
                {
                    return null;
                }
            })
            .Where(ri => ri != null)
            .GroupBy(ri => ri.ISOCurrencySymbol)
            .ToDictionary(x => x.Key, x => x.First()?.CurrencySymbol);
    }

    public static bool TryGetCurrencySymbol(
        string isoCurrencySymbol,
        out string symbol)
    {
        return Map.TryGetValue(isoCurrencySymbol, out symbol);
    }
}
