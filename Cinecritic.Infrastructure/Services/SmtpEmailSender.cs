using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinecritic.Infrastructure.Options;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Cinecritic.Infrastructure.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IOptions<EmailConfigurationOption> _emailConfigurationOption;

        public SmtpEmailSender(IOptions<EmailConfigurationOption> emailConfigurationOption) {
            _emailConfigurationOption = emailConfigurationOption;
        }
            
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string host = _emailConfigurationOption.Value.Server;
            int port = _emailConfigurationOption.Value.Port;
            string fromEmail = _emailConfigurationOption.Value.Email;
            string password = _emailConfigurationOption.Value.Password;
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Cinecritic", email));
            message.To.Add(new MailboxAddress(string.Empty, email));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = htmlMessage,
            };
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(fromEmail, password);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
