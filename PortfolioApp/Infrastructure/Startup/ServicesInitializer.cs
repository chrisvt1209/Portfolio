using PortfolioApp.Infrastructure.Esbuild;
using PortfolioApp.Infrastructure.Helpers;

namespace PortfolioApp.Infrastructure.Startup;

public static partial class ServicesInitializer
{
    public static WebApplicationBuilder RegisterApplicationServices(this WebApplicationBuilder builder,
        IWebHostEnvironment env, IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddControllersWithViews();

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Strict;

            options.OnAppendCookie = cookieContext =>
            {
                cookieContext.CookieOptions.SameSite = SameSiteMode.Strict;
            };
            options.OnDeleteCookie = cookieContext =>
            {
                cookieContext.CookieOptions.SameSite = SameSiteMode.Strict;
            };
            options.Secure = CookieSecurePolicy.Always; // required for chromium-based browsers
        });

        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(183);
        });
        services.AddTransient<ContentHelper>();
        services.AddTransient<IBackofficeUserAccessor, BackofficeUserAccessor>();
        services.ConfigureEsBuild(env.IsDevelopment());

        services.AddResponseCompression();
        return builder;
    }
}
