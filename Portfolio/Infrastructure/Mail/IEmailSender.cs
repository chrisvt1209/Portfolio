using System.Net.Mail;

namespace Portfolio.Infrastructure.Mail;

public interface IEmailSender
{
    MailMessage SendEmail(string fromEmailAddress,
        string[] toEmailAddresses,
        string emailSubject,
        string emailBody,
        IEnumerable<string>? ccEmailAddresses = null,
        IEnumerable<string>? bccEmailAddresses = null,
        IEnumerable<Attachment>? attachments = null,
        IDictionary<string, string?>? replacements = null);
}