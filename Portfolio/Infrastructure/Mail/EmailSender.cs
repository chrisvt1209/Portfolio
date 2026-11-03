using System.Net;
using System.Net.Mail;
using Portfolio.Infrastructure.Extensions;

namespace Portfolio.Infrastructure.Mail;

public class EmailSender : IEmailSender
{
    private const string SupportEmailAddress = "";
    private const string DefaultBccEmailAddress = "";
    private const string DefaultHost = "127.0.0.1";
    private const int DefaultPort = 25;
    
    private readonly IGlobalSettingsService _globalSettings;

    public EmailSender(IGlobalSettingsService globalSettings)
    {
        _globalSettings = globalSettings;
    }

    public MailMessage SendEmail(string fromEmailAddress, string[] toEmailAddresses, string emailSubject, string emailBody,
        IEnumerable<string>? ccEmailAddresses = null, IEnumerable<string>? bccEmailAddresses = null, IEnumerable<Attachment>? attachments = null,
        IDictionary<string, string?>? replacements = null)
    {
        if (string.IsNullOrWhiteSpace(fromEmailAddress))
        {
            throw new ArgumentException(
                "A sender email address is required.",
                nameof(fromEmailAddress));
        }

        ArgumentNullException.ThrowIfNull(toEmailAddresses);

        if (toEmailAddresses.Length == 0)
        {
            throw new ArgumentException(
                "At least one recipient email address is required.",
                nameof(toEmailAddresses));
        }

        var email = ComposeMessage(
            fromEmailAddress,
            toEmailAddresses,
            ccEmailAddresses,
            bccEmailAddresses,
            emailSubject,
            emailBody,
            attachments,
            replacements);

        using var smtpClient = CreateSmtpClient();
        smtpClient.Send(email);

        return email;
    }
    
    private MailMessage ComposeMessage(
        string fromEmailAddress,
        IEnumerable<string?> toEmailAddresses,
        IEnumerable<string?>? ccEmailAddresses,
        IEnumerable<string?>? bccEmailAddresses,
        string emailSubject,
        string emailBody,
        IEnumerable<Attachment>? attachments = null,
        IDictionary<string, string?>? replacements = null)
    {
        var email = new MailMessage
        {
            From = new MailAddress(fromEmailAddress),
            IsBodyHtml = true,
            Subject = ApplyReplacements(emailSubject, replacements),
            Body = ApplyReplacements(emailBody, replacements).ConvertLinebreaks()
        };

        if (_globalSettings.IsTestEnv)
        {
            email.Subject = $"**TEST** {email.Subject}";
        }

        var toAddresses = toEmailAddresses.ToList();
        var isSupportEmail = toAddresses.Any(IsSupportEmail);

        AddToRecipients(email, toAddresses, isSupportEmail);
        AddCcRecipients(email, ccEmailAddresses, isSupportEmail);
        AddBccRecipients(email, bccEmailAddresses, isSupportEmail);

        email.Bcc.Add(DefaultBccEmailAddress);

        if (attachments != null)
        {
            foreach (var attachment in attachments)
            {
                email.Attachments.Add(attachment);
            }
        }

        return email;
    }

    private void AddToRecipients(
        MailMessage email,
        IEnumerable<string?> addresses,
        bool isSupportEmail)
    {
        if (!_globalSettings.IsTestEnv)
        {
            AddAddresses(email.To, addresses);
            return;
        }

        if (isSupportEmail)
        {
            email.To.Add(SupportEmailAddress);
            return;
        }

        AddAllowedAddresses(email.To, addresses);

        email.To.Add(_globalSettings.TestEmailAddress);
    }

    private void AddCcRecipients(
        MailMessage email,
        IEnumerable<string?>? addresses,
        bool isSupportEmail)
    {
        if (isSupportEmail)
        {
            return;
        }

        if (!_globalSettings.IsTestEnv)
        {
            AddAddresses(email.CC, addresses);
            return;
        }

        AddAllowedAddresses(email.CC, addresses);
    }

    private void AddBccRecipients(
        MailMessage email,
        IEnumerable<string?>? addresses,
        bool isSupportEmail)
    {
        if (isSupportEmail)
        {
            return;
        }

        if (!_globalSettings.IsTestEnv)
        {
            AddAddresses(email.Bcc, addresses);
            return;
        }

        AddAllowedAddresses(email.Bcc, addresses);
    }

    private void AddAddresses(
        MailAddressCollection recipients,
        IEnumerable<string?>? addresses)
    {
        if (addresses == null)
        {
            return;
        }

        foreach (var address in addresses)
        {
            if (!string.IsNullOrWhiteSpace(address))
            {
                recipients.Add(address);
            }
        }
    }

    private void AddAllowedAddresses(
        MailAddressCollection recipients,
        IEnumerable<string?>? addresses)
    {
        if (addresses == null)
        {
            return;
        }

        var allowedDomains = _globalSettings.AllowDomainsInTest;

        if (!allowedDomains.Any())
        {
            return;
        }

        foreach (var address in addresses)
        {
            if (!string.IsNullOrWhiteSpace(address) &&
                IsAllowedDomain(address, allowedDomains))
            {
                recipients.Add(address);
            }
        }
    }

    private static bool IsAllowedDomain(
        string emailAddress,
        IEnumerable<string> allowedDomains)
    {
        var separatorIndex = emailAddress.LastIndexOf('@');

        if (separatorIndex < 0)
        {
            return false;
        }

        var emailDomain = emailAddress[(separatorIndex + 1)..];

        return allowedDomains.Any(domain =>
            !string.IsNullOrWhiteSpace(domain) &&
            string.Equals(
                emailDomain,
                domain.Trim().TrimStart('@'),
                StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsSupportEmail(string? emailAddress)
    {
        return string.Equals(
            emailAddress,
            SupportEmailAddress,
            StringComparison.OrdinalIgnoreCase);
    }

    private static string ApplyReplacements(
        string template,
        IDictionary<string, string?>? replacements)
    {
        return replacements == null || replacements.Count == 0
            ? template
            : ReplaceKeys(template, replacements);
    }

    private static string ReplaceKeys(
        string template,
        IDictionary<string, string?> entries)
    {
        foreach (var entry in entries)
        {
            template = template.Replace(
                entry.Key,
                entry.Value ?? string.Empty);
        }

        return template;
    }

    private SmtpClient CreateSmtpClient()
    {
        var settings = _globalSettings.MailSettings;
        var smtpClient = new SmtpClient
        {
            DeliveryFormat = SmtpDeliveryFormat.International,
            EnableSsl = !string.IsNullOrWhiteSpace(settings["enableSsl"])
                && Convert.ToBoolean(settings["enableSsl"])
        };

        if (settings["deliveryMethod"] ==
            nameof(SmtpDeliveryMethod.SpecifiedPickupDirectory))
        {
            smtpClient.DeliveryMethod =
                SmtpDeliveryMethod.SpecifiedPickupDirectory;

            smtpClient.PickupDirectoryLocation =
                settings["specifiedPickupDirectory:pickupDirectoryLocation"];
        }
        else
        {
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Host = settings["host"] ?? DefaultHost;
            smtpClient.Port = Convert.ToInt32(
                settings["port"] ?? DefaultPort.ToString());

            smtpClient.Credentials = new NetworkCredential(
                settings["username"],
                settings["password"]);
        }

        return smtpClient;
    }
}