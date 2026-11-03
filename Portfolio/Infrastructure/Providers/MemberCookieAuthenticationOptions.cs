using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Options;

namespace Portfolio.Infrastructure.Providers;

public class MemberCookieAuthenticationOptions : IConfigureNamedOptions<CookieAuthenticationOptions>
{
    private readonly IConfiguration _configuration;

    public MemberCookieAuthenticationOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(string? name, CookieAuthenticationOptions options)
    {
        if (name == IdentityConstants.ApplicationScheme || name == IdentityConstants.ExternalScheme ||
            name == IISDefaults.AuthenticationScheme)
            Configure(options);
    }

    public void Configure(CookieAuthenticationOptions options)
    {
        var sessionExpireMinutes = _configuration.GetValue<int>("Portfolio:Session:ExpireMinutes");
        if (sessionExpireMinutes == 0)
        {
            sessionExpireMinutes = 10;
        }

        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.AccessDeniedPath = "/umbraco/surface/AuthSurface/Logout";
        options.Cookie.Name = "__Host-.Portfolio.Identity.Application";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionExpireMinutes);
        options.SlidingExpiration = true;

        options.Events.OnCheckSlidingExpiration = context =>
        {
            var path = context.Request.Path.Value;

            if (!string.IsNullOrEmpty(path) &&
                path.StartsWith("/api/session/status", StringComparison.OrdinalIgnoreCase))
            {
                context.ShouldRenew = false;
            }

            return Task.CompletedTask;
        };

    }
}
