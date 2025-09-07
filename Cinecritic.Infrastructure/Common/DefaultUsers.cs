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
            DisplayName = "Mykyta",
            Email = "nikitatitarenko81@gmail.com",
            NormalizedEmail = "NIKITATITARENKO81@.GMAIL.COM",
            UserName = "nikitatitarenko81@gmail.com",
            NormalizedUserName = "NIKITATITARENKO81@.GMAIL.COM",
            EmailConfirmed = true,
        };
    }
}
