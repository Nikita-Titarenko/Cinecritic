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
        public static readonly IdentityRole UserRole = new IdentityRole
        {
            Id = "ec4742cf-d15e-422f-aaa2-2b9e9fac58f9",
            Name = "User",
            NormalizedName = "USER"
        };

        public static readonly IdentityRole ManagerRole = new IdentityRole
        {
            Id = "a51c8989-2651-49ff-8c93-35edd02e546f",
            Name = "Manager",
            NormalizedName = "MANAGER"
        };
    }
}
