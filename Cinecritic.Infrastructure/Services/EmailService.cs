using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinecritic.Application.Services.Email;
using Cinecritic.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Cinecritic.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender<ApplicationUser> _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailService(IEmailSender<ApplicationUser> emailService, UserManager<ApplicationUser> userManager)
        {
            _emailSender = emailService;
            _userManager = userManager;
        }

        public async Task SendConfirmationLinkAsync(string email, string confirmationLink)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }
            await _emailSender.SendConfirmationLinkAsync(user, email, confirmationLink);
        }

        public async Task SendPasswordResetCodeAsync(string email, string resetCode)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }
            await _emailSender.SendPasswordResetCodeAsync(user, email, resetCode);
        }

        public async Task SendPasswordResetLinkAsync(string email, string resetLink)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }
            await _emailSender.SendPasswordResetLinkAsync(user, email, resetLink);
        }
    }
}
