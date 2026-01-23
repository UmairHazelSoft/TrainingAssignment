using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Training_Assignment.Services.Implementation
{
    /// <summary>
    /// Simple email sender for demo/testing purposes.
    /// Implements IEmailSender required by Identity for email confirmation.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Get SMTP settings from appsettings.json
            string smtpHost = _configuration["Email:SmtpHost"];
            int smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            string smtpUser = _configuration["Email:SmtpUser"];
            string smtpPass = _configuration["Email:SmtpPass"];
            string fromEmail = _configuration["Email:FromEmail"];

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}
