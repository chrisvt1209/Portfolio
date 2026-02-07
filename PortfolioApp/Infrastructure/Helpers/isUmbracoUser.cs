using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace PortfolioApp.Infrastructure.Helpers;

public interface IBackofficeUserAccessor
{
    bool isLoggedIn { get; }
}

public class BackofficeUserAccessor : IBackofficeUserAccessor
{
    private readonly IOptionsSnapshot<CookieAuthenticationOptions> _cookieOptionsSnapshot;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BackofficeUserAccessor(
        IOptionsSnapshot<CookieAuthenticationOptions> cookieOptionsSnapshot,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _cookieOptionsSnapshot = cookieOptionsSnapshot;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool isLoggedIn
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var cookieOptionsSnapshot = httpContext.RequestServices.GetRequiredService<IOptionsSnapshot<CookieAuthenticationOptions>>();
            var cookieOptions = cookieOptionsSnapshot.Get(Umbraco.Cms.Core.Constants.Security.BackOfficeAuthenticationType);
            var backOfficeCookie = httpContext.Request.Cookies[cookieOptions.Cookie.Name!];
            var cookieExists = !string.IsNullOrWhiteSpace(backOfficeCookie);

            return cookieExists;
        }
    }
}
