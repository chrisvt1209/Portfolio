using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace PortfolioApp.Infrastructure.Extensions;

public static class TempDataExtensions
{
    public static void Set<T>(this ITempDataDictionary tempData, string key, T value)
    {
        tempData[key] = JsonConvert.SerializeObject(value);
    }

    public static T? Get<T>(this ITempDataDictionary tempData, string key)
    {
        var value = tempData[key]?.ToString();
        return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }
}