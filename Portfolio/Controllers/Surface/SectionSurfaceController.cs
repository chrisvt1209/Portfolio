using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.Umbraco;
using Portfolio.Models.ViewModels;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace Portfolio.Controllers.Surface;

public class SectionSurfaceController : SurfaceController
{
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly IPublishedValueFallback _publishedValueFallback;
    
    public SectionSurfaceController(
        IUmbracoContextAccessor umbracoContextAccessor, 
        IUmbracoDatabaseFactory databaseFactory, 
        ServiceContext services, 
        AppCaches appCaches, 
        IProfilingLogger profilingLogger, 
        IPublishedUrlProvider publishedUrlProvider, IPublishedContentQuery publishedContentQuery, IPublishedValueFallback publishedValueFallback) 
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _publishedContentQuery = publishedContentQuery;
        _publishedValueFallback = publishedValueFallback;
    }

    public IActionResult GetAbout(int position = 1) =>
        RenderSection(About.ModelTypeAlias, "_About.cshtml", position,
            content => new AboutViewModel(content, _publishedValueFallback));
    
    public IActionResult GetExperience(int position = 1) =>
        RenderSection(Experience.ModelTypeAlias, "_Experience.cshtml", position,
            content => new ExperienceViewModel(content, _publishedValueFallback));
    
    public IActionResult GetProjects(int position = 1) =>
        RenderSection(Projects.ModelTypeAlias, "_Projects.cshtml", position,
            content => new ProjectsViewModel(content, _publishedValueFallback));
    
    public IActionResult GetMoreProjects(int position = 1) =>
        RenderSection(MoreProjects.ModelTypeAlias, "_MoreProjects.cshtml", position,
            content => new MoreProjectsViewModel(content, _publishedValueFallback));

    private IActionResult RenderSection(string contentTypeAlias, string viewName, int position,
        Func<IPublishedContent, object> createModel)
    {
        IPublishedContent? content = FindSection(contentTypeAlias);
        
        ViewData["SectionNumber"] = $"{Math.Max(position, 1):00}.";

        return content is null
            ? NotFound()
            : PartialView($"/Views/Partials/{viewName}", createModel(content));
    }
    
    private IPublishedContent? FindSection(string contentTypeAlias) =>
        _publishedContentQuery
            .ContentAtRoot()
            .FirstOrDefault(root => root.ContentType.Alias == Home.ModelTypeAlias)?
            .Children()
            .FirstOrDefault(child => child.ContentType.Alias == contentTypeAlias);
}