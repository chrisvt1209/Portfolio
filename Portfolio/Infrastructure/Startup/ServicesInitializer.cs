using Portfolio.Infrastructure.Extensions;
using Portfolio.Infrastructure.Handlers;
using Portfolio.Infrastructure.Helpers;
using Portfolio.Services;
using Portfolio.Infrastructure.Helpers;

namespace Portfolio.Infrastructure.Startup;

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
            options.Secure = CookieSecurePolicy.Always; // required for Chromium-based browsers
        });
        services.AddSession(options =>
        {
            options.Cookie.Name = "__Host-.Portfolio.Session";
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.None;
        });
        services.AddAntiforgery(options =>
        {
            options.Cookie.Name = "__Host-.Portfolio.Antiforgery";
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.SuppressXFrameOptionsHeader = true;
        });
        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(183);
        });
        services.AddTransient<ContentHelper>();
        services.AddScoped<UserService>();
        services.AddTransient<IBackofficeUserAccessor, BackofficeUserAccessor>();
        services.AddMailServices();
        services.ConfigureEsBuild(env.IsDevelopment());

        services.AddExceptional(builder.Configuration.GetSection("Exceptional"));
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddResponseCompression();
        return builder;
    }
}
