using Portfolio.Infrastructure.Mail;

namespace Portfolio.Infrastructure.Extensions;

public static class MailExtensions
{
    public static IServiceCollection AddMailServices(this IServiceCollection services)
    {
        services.AddScoped<IGlobalSettingsService, GlobalSettingsService>();
        services.AddScoped<IEmailSender, EmailSender>();

        return services;
    }
}