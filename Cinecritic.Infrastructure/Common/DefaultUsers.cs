using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinecritic.Infrastructure.Data;

namespace Cinecritic.Infrastructure.Common
{
    public static class DefaultUsers
    {
        public static readonly ApplicationUser Manager = new ApplicationUser
        {
            Id = "363636b4-141c-4de1-a9be-84b40d93b0b0",
            DisplayName = "Mykyta",
            Email = "nikitatitarenko81@gmail.com",
            NormalizedEmail = "NIKITATITARENKO81@GMAIL.COM",
            UserName = "nikitatitarenko81@gmail.com",
            NormalizedUserName = "NIKITATITARENKO81@GMAIL.COM",
            EmailConfirmed = true,
        };
    }
}
