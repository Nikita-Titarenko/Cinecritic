using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinecritic.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinecritic.Infrastructure.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var user = DefaultUsers.Manager;
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "77228Glnik!");
            builder.HasData(user);
        }
    }
}