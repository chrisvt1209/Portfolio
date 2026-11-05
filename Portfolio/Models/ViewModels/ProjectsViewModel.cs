using Portfolio.Models.Umbraco;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Portfolio.Models.ViewModels;

public class ProjectsViewModel : Projects
{
    public ProjectsViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }
}