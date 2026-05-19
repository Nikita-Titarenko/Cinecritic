using Cinecritic.Infrastructure.Data;

namespace Cinecritic.Infrastructure.Common
{
    public static class DefaultUsers
    {
        public static readonly ApplicationUser Manager = new ApplicationUser
        {
            Id = 1000,
            DisplayName = "Mykyta",
            Email = "nikitatitarenko81@gmail.com",
            NormalizedEmail = "NIKITATITARENKO81@GMAIL.COM",
            UserName = "nikitatitarenko81@gmail.com",
            NormalizedUserName = "NIKITATITARENKO81@GMAIL.COM",
            EmailConfirmed = true,
            SecurityStamp = "f5b48743-a597-42e7-8d83-62af1a26ab5f",
            ConcurrencyStamp = "3906ae6c-f560-495d-9a04-442d4e781053",
        };
    }
}
