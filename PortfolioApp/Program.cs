using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Newtonsoft.Json.Linq;
using PortfolioApp.Infrastructure.Startup;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

IWebHostEnvironment _env = builder.Environment;
IServiceCollection services = builder.Services;

builder.RegisterApplicationServices(_env, services);

WebApplication app = builder.Build();

await app.BootUmbracoAsync();

app.ConfigureMiddleware(_env);

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

_ = Task.Run(async () =>
{
    if (_env.IsDevelopment())
    {
        var appSettingsPath = Path.Combine(builder.Environment.ContentRootPath, "appsettings.Development.json");
        if (File.Exists(appSettingsPath))
        {
            var json = JObject.Parse(File.ReadAllText(appSettingsPath));
            if (json["Umbraco"]["CMS"]["WebRouting"]["UmbracoApplicationUrl"] == null)
            {
                await Task.Delay(1000);
                var server = app.Services.GetService<IServer>();
                var addressesFeature = server?.Features.Get<IServerAddressesFeature>();
                if (addressesFeature != null && addressesFeature.Addresses.Any())
                {
                    var url = addressesFeature.Addresses.FirstOrDefault(a => a.StartsWith("https://localhost:"));

                    if (!String.IsNullOrEmpty(url))
                    {
                        var port = new Uri(url).Port;
                        json["Umbraco"]["CMS"]["WebRouting"]["UmbracoApplicationUrl"] = $"https://localhost:{port}";
                    }

                    File.WriteAllText(appSettingsPath, json.ToString());
                }
            }
        }
    }
});

await app.RunAsync();
