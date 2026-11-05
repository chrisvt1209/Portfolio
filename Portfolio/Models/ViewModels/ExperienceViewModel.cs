using Portfolio.Models.Umbraco;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Portfolio.Models.ViewModels;

public class ExperienceViewModel : Experience
{
    public ExperienceViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }
}