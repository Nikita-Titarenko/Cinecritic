using System.Reflection;
using Cinecritic.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinecritic.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieType> MovieTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Movie>().Property(m => m.ReleaseDate).IsRequired(false);
            builder.Entity<Movie>().Property(m => m.Title).HasMaxLength(200);
            builder.Entity<Movie>().Property(m => m.Description).IsRequired(false).HasMaxLength(2000);
            builder.Entity<Movie>().HasOne(mt => mt.MovieType).WithMany(m => m.Movies).HasForeignKey(m => m.MovieTypeId);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
