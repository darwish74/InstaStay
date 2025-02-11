using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace Models.Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword)
            };
            return client.SendMailAsync(
                new MailMessage(from: _emailSettings.SenderEmail,
                                to: email,
                                subject,
                                message)
                {
                    IsBodyHtml = true
                });
        }
    }
}
