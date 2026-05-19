using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Cinecritic.Infrastructure.Common
{
    public static class DefaultRoles
    {
        public static readonly IdentityRole<int> UserRole = new IdentityRole<int>
        {
            Id = 1,
            Name = "User",
            NormalizedName = "USER"
        };

        public static readonly IdentityRole<int> ManagerRole = new IdentityRole<int>
        {
            Id = 2,
            Name = "Manager",
            NormalizedName = "MANAGER"
        };
    }
}
