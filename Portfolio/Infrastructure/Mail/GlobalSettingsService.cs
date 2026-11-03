namespace Portfolio.Infrastructure.Mail;

public class GlobalSettingsService : IGlobalSettingsService
{
    private readonly IConfiguration? _configuration;

    public GlobalSettingsService(IConfiguration? configuration)
    {
        _configuration = configuration;
    }

    public bool IsTestEnv
    {
        get
        {
            try
            {
                return _configuration?.GetValue<bool>("TestInd") ?? true;
            }
            catch
            {
                return false;
            }
        }
    }
    
    public string TestEmailAddress => _configuration?.GetValue<string>("MailToIfTestEnv") ?? "";
    
    public string[] AllowDomainsInTest
    {
        get
        {
            var allowedDomainsInTest = _configuration?.GetSection("allowedDomainsInTest").Get<string[]>();
            return allowedDomainsInTest ?? [];
        }
    }
    
    public IConfigurationSection MailSettings => _configuration?.GetSection("mailSettings:smtp")
                                                 ?? throw new NullReferenceException("mailSettings:smtp was no set");
}