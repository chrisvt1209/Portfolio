using Portfolio.Models.Umbraco;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Portfolio.Models.ViewModels;

public class AboutViewModel : About
{
    public AboutViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }
}