using PortfolioApp.Infrastructure.Esbuild;
using PortfolioApp.Infrastructure.Extensions;

namespace PortfolioApp.Infrastructure.Startup;

public static class MiddlewareInitializer
{
    public static WebApplication ConfigureMiddleware(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
            app.UseExceptionHandler("/errors");
        }

        app.UseCookiePolicy(
                           new CookiePolicyOptions
                           {
                               Secure = CookieSecurePolicy.Always
                           });

        app.UseXContentTypeOptions();
        app.UseXXssProtection(options => options.EnabledWithBlockMode());

        app.Use((context, next) =>
        {
            context.Response.Headers.Append("Referrer-Policy", "same-origin");
            context.Response.Headers.Append("Permissions-Policy",
                "accelerometer=(self), autoplay=(self),  camera=(self), cross-origin-isolated=(self), display-capture=(self), encrypted-media=(self), fullscreen=(self), geolocation=(self), gyroscope=(self), keyboard-map=(self), magnetometer=(self), microphone=(self), midi=(self),  payment=(self), picture-in-picture=(self), publickey-credentials-get=(self), screen-wake-lock=(self), sync-xhr=(self), usb=(self), web-share=(self), xr-spatial-tracking=(self), clipboard-read=(self), clipboard-write=(self), gamepad=(self), hid=(self), idle-detection=(self),  serial=(self)");

            var url = context.Request.Path.ToString();

            var staticUrls = new[]
                { ".js", ".css", ".png", ".jpg", ".pdf", ".jpeg", ".mp4", ".webp", ".woff2", "woff", ".gif", ".svg" };

            if (staticUrls.Any(x => url.EndsWith(x)))
            {
                context.Response.Headers.Append("Cache-Control", "max-age=31536000");
            }

            return next();
        });

        app.UseSecurityPolicy(env.IsDevelopment());

        app.UseSession();
        app.UseHttpsRedirection();
        app.UseResponseCompression();
        app.UseStaticFiles();

        app.UseRouting();
        if (env.IsDevelopment())
        {
            app.UseEsbuild(env.WebRootPath);
        }
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
