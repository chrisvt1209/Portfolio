using PortfolioApp.Models.Umbraco;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace PortfolioApp.Infrastructure.Extensions;

public static class ContentExtensions
{
    public static string GetAnchor(this IPublishedContent content)
    {
        return content.ContentType.Alias switch
        {
            AboutMe.ModelTypeAlias => "#about",
            Experience.ModelTypeAlias => "#experience",
            Projects.ModelTypeAlias => "#projects",
            _ => "unknown"
        };
    }
}
