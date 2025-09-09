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

            builder.Entity<MovieUser>()
    .HasKey(mu => new { mu.MovieId, mu.UserId });

            builder.Entity<MovieUser>()
                .HasOne(mu => mu.Movie)
                .WithMany(m => m.MovieUsers)
                .HasForeignKey(mu => mu.MovieId);

            builder.Entity<MovieUser>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.MovieUsers)
                .HasForeignKey(mu => mu.UserId);


            builder.Entity<WatchList>()
                .HasKey(wl => new { wl.MovieId, wl.UserId });

            builder.Entity<WatchList>()
                .HasOne(wl => wl.Movie)
                .WithMany(m => m.WatchLists)
                .HasForeignKey(wl => wl.MovieId);

            builder.Entity<WatchList>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.WatchLists)
                .HasForeignKey(wl => wl.UserId);

            //builder.Entity<MovieUser>()
            //    .HasOne(mt => mt.Movie)
            //    .WithMany(m => m.MovieUsers)
            //    .HasForeignKey(m => m.MovieId);
            //builder.Entity<MovieUser>()
            //    .HasOne<ApplicationUser>()
            //    .WithMany()
            //.HasPrincipalKey(u => u.Id);
            //builder.Entity<MovieUser>().HasKey(mu => new { mu.MovieId, mu.UserId });

            //builder.Entity<WatchList>()
            //    .HasOne(mt => mt.Movie)
            //    .WithMany(m => m.WatchLists)
            //    .HasForeignKey(m => m.MovieId);
            //builder.Entity<WatchList>()
            //    .HasOne<ApplicationUser>()
            //    .WithMany()
            //    .HasForeignKey(m => m.UserId)
            //.HasPrincipalKey(u => u.Id);
            //builder.Entity<WatchList>().HasKey(wl => new { wl.MovieId, wl.UserId });

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
