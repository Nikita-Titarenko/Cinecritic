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
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(DefaultRoles.UserRole);
            builder.HasData(DefaultRoles.ManagerRole);
        }
    }
}
