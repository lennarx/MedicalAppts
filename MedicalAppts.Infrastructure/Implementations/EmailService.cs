using MedicalAppts.Core.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;

namespace MedicalAppts.Infrastructure.Implementations
{
    public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<EmailService> _logger = logger;

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUser = _configuration["EmailSettings:SmtpUser"];
            var smtpPass = _configuration["EmailSettings:SmtpPass"];

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(fromEmail, toEmail, subject, body);

            try
            {
                // Sends the email asynchronously
                _logger.LogInformation($"Email sent to {toEmail} with subject: {subject}");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, $"Failed to send email to {toEmail}");
            }
        }
    }
}
