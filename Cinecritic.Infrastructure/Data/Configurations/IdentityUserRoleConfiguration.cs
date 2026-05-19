using Cinecritic.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinecritic.Infrastructure.Data.Configurations
{
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
        {
            builder.HasData(new IdentityUserRole<int>
            {
                UserId = DefaultUsers.Manager.Id,
                RoleId = DefaultRoles.ManagerRole.Id,
            });
        }
    }
}