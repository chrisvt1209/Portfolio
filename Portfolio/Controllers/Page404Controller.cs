using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Portfolio.Models.ViewModels;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace Portfolio.Controllers;

public class Page404Controller : RenderController
{
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly ServiceContext _serviceContext;
    private readonly IPropertyRenderingContextAccessor _propertyRenderingContextAccessor;
    
    public Page404Controller(
        ILogger<RenderController> logger, 
        ICompositeViewEngine compositeViewEngine, 
        IUmbracoContextAccessor umbracoContextAccessor, IVariationContextAccessor variationContextAccessor, ServiceContext serviceContext, IPropertyRenderingContextAccessor propertyRenderingContextAccessor) 
        : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
        _variationContextAccessor = variationContextAccessor;
        _serviceContext = serviceContext;
        _propertyRenderingContextAccessor = propertyRenderingContextAccessor;
    }

    public override IActionResult Index()
    {
        if (CurrentPage != null)
        {
            var viewModel = new Page404ViewModel(CurrentPage,
                new PublishedValueFallback(_serviceContext, _variationContextAccessor, _propertyRenderingContextAccessor));
            return CurrentTemplate(viewModel);
        }
        return CurrentTemplate(this);
    }
}