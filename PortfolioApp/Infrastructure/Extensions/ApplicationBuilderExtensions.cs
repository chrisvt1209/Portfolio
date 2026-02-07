namespace PortfolioApp.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSecurityPolicy(this IApplicationBuilder app, bool isDevelopment)
    {
        //CSP security headers
        return app
            .UseWhen(x => !IsBackOffice(x), a =>
            {
                var policyCollection = new HeaderPolicyCollection()
                    .RemoveServerHeader()
                    .AddContentSecurityPolicy(builder =>
                    {
                        builder.AddObjectSrc().None();
                        builder.AddFormAction().Self();
                    });
                policyCollection
                    .AddContentSecurityPolicy(builder =>
                    {
                        builder
                            .AddDefaultSrc()
                            .Self();

                        builder.AddScriptSrc()
                            .Self()
                            .From("https://*.fontawesome.com")
                            .From("blob:")
                            .WithNonce();

                        builder.AddStyleSrc()
                            .Self()
                            .From("https://*.fontawesome.com")
                            .WithNonce();

                        builder.AddImgSrc()
                            .Self()
                            .From("data:");

                        builder.AddFormAction()
                            .Self();

                        builder.AddFontSrc()
                            .Self()
                            .From("https://*.fontawesome.com");


                        var connectSrc = builder.AddConnectSrc()
                            .Self()
                            .From("https://*.fontawesome.com")
                            .From("ws:")
                            .From("wss:");

                        if (isDevelopment)
                        {
                            connectSrc
                                .From("http://localhost:*")
                                .From("https://localhost:*");
                        }

                        builder.AddObjectSrc()
                            .None();

                        builder.AddBaseUri()
                            .Self();

                    });

                a.UseSecurityHeaders(policyCollection);
            })
            .UseWhen(IsBackOffice, a =>
            {
                var policyCollection = new HeaderPolicyCollection()
                    .RemoveServerHeader()
                    .AddContentSecurityPolicy(builder =>
                    {
                        builder.AddObjectSrc().None();
                        builder.AddFormAction().Self();
                        builder.AddFrameAncestors().None();
                    });
                policyCollection
                    .AddContentSecurityPolicy(builder =>
                    {
                        builder.AddDefaultSrc()
                            .From("https://dashboard.tambien.nl")
                            .Self();

                        builder.AddScriptSrc()
                            .Self().UnsafeInline().UnsafeEval();

                        builder.AddStyleSrc()
                            .Self().UnsafeInline();

                        builder.AddImgSrc()
                            .Self()
                            .Data()
                            .From("dashboard.umbraco.com")
                            .From("our.umbraco.com");

                        builder.AddConnectSrc()
                            .Self()
                            .From("wss:")
                            .From("our.umbraco.com");

                        builder.AddObjectSrc()
                            .None();

                        builder.AddFrameSrc()
                            .From("marketplace.umbraco.com")
                            .From("https://dashboard.tambien.nl")
                            .Self();
                    });

                a.UseSecurityHeaders(policyCollection);
            });
    }

    private static bool IsBackOffice(HttpContext context) => context.Request.Path.StartsWithSegments(new PathString("/umbraco")) || context.Request.Path.StartsWithSegments(new PathString("/umbraco/install")) || context.Request.Path.StartsWithSegments(new PathString("/exceptions"));
}
