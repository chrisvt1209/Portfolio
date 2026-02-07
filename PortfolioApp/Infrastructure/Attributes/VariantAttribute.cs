using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace PortfolioApp.Infrastructure.Attributes;

public class VariantAttribute : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var req = context.HttpContext.Request;
        var culture = req.Headers["Tmb-Lang"].ToString();
        var variationContextAccessor =
            context.HttpContext.RequestServices.GetRequiredService<IVariationContextAccessor>();
        variationContextAccessor.VariationContext =
            new VariationContext(string.IsNullOrWhiteSpace(culture) ? "nl" : culture);

        var actualCulture = culture switch
        {
            "nl" => "nl-NL",
            "en" => "en-US",
            "de" => "de-DE",
            _ => "nl-NL"
        };
        var ci = new CultureInfo(actualCulture);
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // do nothing
    }
}
