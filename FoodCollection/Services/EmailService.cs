using FoodCollection.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace FoodCollection.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(EmailModel email)
        {
            var smtpClient = new SmtpClient(_config["EmailSettings:MailServer"])
            {
                Port = int.Parse(_config["EmailSettings:MailPort"]),
                Credentials = new NetworkCredential(
                    _config["EmailSettings:Username"],
                    _config["EmailSettings:Password"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["EmailSettings:SenderEmail"], _config["EmailSettings:SenderName"]),
                Subject = email.Subject,
                Body = email.Message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email.ToEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
