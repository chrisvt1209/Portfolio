namespace Portfolio.Infrastructure.Mail;

public interface IGlobalSettingsService
{
    bool IsTestEnv { get; }
    string TestEmailAddress { get; }
    string[] AllowDomainsInTest { get; }
    IConfigurationSection MailSettings { get; }
}