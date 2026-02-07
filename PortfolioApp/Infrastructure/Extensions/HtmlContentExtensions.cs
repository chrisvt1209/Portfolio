using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;

namespace PortfolioApp.Infrastructure.Extensions;

public static class HtmlContentExtensions
{
    public static string GetString(this IHtmlContent content)
    {
        using var writer = new StringWriter();
        content.WriteTo(writer, HtmlEncoder.Default);
        return writer.ToString();
    }
}
