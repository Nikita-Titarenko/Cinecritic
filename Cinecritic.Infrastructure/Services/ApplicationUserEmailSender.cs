using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinecritic.Infrastructure.Data;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace Cinecritic.Infrastructure.Services
{
    public class ApplicationUserEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string tempaltePath = "templates";
        private readonly string emailTempalte = "email-layout.html";

        public ApplicationUserEmailSender(IEmailSender emailSender, IWebHostEnvironment webHostEnvironment) {
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task<string?> CreateHtmlAsync(string title, string text, string link, string linkText)
        {
            string templatePath = Path.Combine(_webHostEnvironment.WebRootPath, tempaltePath, emailTempalte);
            var html = await File.ReadAllTextAsync(templatePath);
            if (string.IsNullOrEmpty(html))
            {
                return null;
            }

            html = html.Replace("{Title}", title)
                .Replace("{Text}", text)
                .Replace("{Link}", link)
                .Replace("{LinkText}", linkText);
            
            return html;
        }

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            string title = "Confirm registration on Cinecritic";
            var html = await CreateHtmlAsync(title, "Confirm your email to end registration", confirmationLink, "Confirm Email");
            if (string.IsNullOrEmpty(html))
            {
                return;
            }
            await _emailSender.SendEmailAsync(email, title, html);
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }
    }
}
