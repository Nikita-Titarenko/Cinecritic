using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinecritic.Application.Services.Email
{
    public interface IEmailService
    {
        Task SendConfirmationLinkAsync(string email, string confirmationLink);

        Task SendPasswordResetLinkAsync(string email, string resetLink);

        Task SendPasswordResetCodeAsync(string email, string resetCode);
    }
}