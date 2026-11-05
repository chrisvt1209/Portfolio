using Portfolio.Models.Umbraco;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Portfolio.Models.ViewModels;

public class Page404ViewModel : Page404
{
    public Page404ViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }
}