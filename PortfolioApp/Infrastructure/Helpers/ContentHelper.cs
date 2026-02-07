using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;

namespace PortfolioApp.Infrastructure.Helpers;

public class ContentHelper
{
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    public ContentHelper(IUmbracoContextAccessor umbracoContextAccessor)
    {
        _umbracoContextAccessor = umbracoContextAccessor;
    }

    private Task<IPublishedContentCache> GetContentHelper()
    {
        if (_umbracoContextAccessor.TryGetUmbracoContext(out var context) == false)
        {
            throw new Exception("unable to get content");
        }

        var content = context.Content;

        if (content == null)
        {
            throw new NullReferenceException("content is null");
        }

        return Task.FromResult(content);
    }

    public async Task<T?> GetByKey<T>(Guid key) where T : class, IPublishedContent
    {
        var content = await GetContentHelper();
        return content.GetById(key) as T;
    }

    //public async Task<Settings?> GetSettingsAsync()
    //{
    //    var content = await GetContentHelper();
    //    return content.GetAtRoot()
    //        .FirstOrDefault(x => x.ContentType.Alias == Settings.ModelTypeAlias)?
    //        .SafeCast<Settings>();
    //}

}