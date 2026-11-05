using Portfolio.Models;
using Portfolio.Models.Umbraco;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;

namespace Portfolio.Infrastructure.Helpers;

public class ContentHelper
{
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IPublishedModelFactory _publishedModelFactory;
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly IPublishedValueFallback _publishedValueFallback;


    public ContentHelper(IUmbracoContextAccessor umbracoContextAccessor, IPublishedModelFactory publishedModelFactory, IPublishedContentQuery publishedContentQuery, IPublishedValueFallback publishedValueFallback)
    {
        _umbracoContextAccessor = umbracoContextAccessor;
        _publishedModelFactory = publishedModelFactory;
        _publishedContentQuery = publishedContentQuery;
        _publishedValueFallback = publishedValueFallback;
    }

    private IPublishedContentCache GetContentCache()
    {
        if (!_umbracoContextAccessor.TryGetUmbracoContext(out var context))
        {
            throw new InvalidOperationException("Unable to get UmbracoContext.");
        }

        var content = context.Content;
        if (content == null)
        {
            throw new NullReferenceException("UmbracoContext.Content is null.");
        }

        return content;
    }

    public Task<T?> GetByKey<T>(Guid key) where T : class, IPublishedContent
    {
        var cache = GetContentCache();
        var node = cache.GetById(key) as T;
        return Task.FromResult(node);
    }

    private IPublishedContent? GetHomeNode() =>
        _publishedContentQuery
            .ContentAtRoot()
            .FirstOrDefault(x => x.ContentType.Alias == Home.ModelTypeAlias);
    
    private IPublishedContent? GetSettingsNode() =>
        _publishedContentQuery
            .ContentAtRoot()
            .FirstOrDefault(x => x.ContentType.Alias == Settings.ModelTypeAlias);

    private TModel? ToModel<TModel>(IPublishedContent? content)
        where TModel : class, IPublishedContent
        => content?.CreateModel(_publishedModelFactory) as TModel;

    public Task<Home?> GetHomeAsync()
    {
        var cache = GetContentCache();
        var home = ToModel<Home>(GetHomeNode());
        return Task.FromResult(home);
    }
    
    public Task<Settings?> GetSettingsAsync()
    {
        var cache = GetContentCache();
        var settings = ToModel<Settings>(GetSettingsNode());
        return Task.FromResult(settings);
    }

    public Task<SettingsModel?> GetSettingsModelAsync()
    {
        var node = GetSettingsNode();
        var model = node is null ? null : new SettingsModel(node, _publishedValueFallback);
        return Task.FromResult(model);
    }
}
