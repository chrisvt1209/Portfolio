using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;

namespace Portfolio.Controllers.Surface;

public class AuthSurfaceController : SurfaceController
{
    private readonly IMemberSignInManager _memberSignInManager;

    public AuthSurfaceController(
        IUmbracoContextAccessor umbracoContextAccessor, 
        IUmbracoDatabaseFactory databaseFactory, 
        ServiceContext services, 
        AppCaches appCaches, 
        IProfilingLogger profilingLogger, 
        IPublishedUrlProvider publishedUrlProvider, 
        IMemberSignInManager memberSignInManager) 
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _memberSignInManager = memberSignInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        try
        {
            TempData.Clear();
            HttpContext.Session.Clear();
            await _memberSignInManager.SignOutAsync();
        }
        catch (Exception)
        {
            // ignored
        }
        return Redirect("/");
    }
}
