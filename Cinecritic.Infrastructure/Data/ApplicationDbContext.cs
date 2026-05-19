using System.Reflection;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinecritic.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options)
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieType> MovieTypes { get; set; }

        public DbSet<MovieUser> MovieUsers { get; set; }

        public DbSet<WatchList> WatchLists { get; set; }
        
        public decimal GetMovieAverageRating(int movieId) => throw new NotSupportedException();
        
        public IQueryable<TopMovieQueryResult> GetTopMoviesByType(
            int movieTypeId, 
            decimal minRating, 
            int pageNumber, 
            int pageSize, 
            string? applicationUserId)
        {
            return FromExpression(() => GetTopMoviesByType(movieTypeId, minRating, pageNumber, pageSize, applicationUserId));
        }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<TopMovieQueryResult>().HasNoKey();
            
            builder.HasDbFunction(typeof(ApplicationDbContext).GetMethod(nameof(GetMovieAverageRating), new[] { typeof(int) })!)
                .HasName("GetMovieAverageRating")
                .HasSchema("dbo");
            
            builder.HasDbFunction(typeof(ApplicationDbContext).GetMethod(nameof(GetTopMoviesByType))!)
                .HasName("GetTopMoviesByType")
                .HasSchema("dbo");
            
            builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
            
            builder.Entity<IdentityRole<int>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("ApplicationUserRoles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("ApplicationUserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("ApplicationUserLogins");
            builder.Entity<IdentityUserToken<int>>().ToTable("ApplicationUserTokens");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            
            builder.Entity<Movie>().Property(m => m.ReleaseDate).IsRequired(false);
            builder.Entity<Movie>().Property(m => m.Title).HasMaxLength(200);
            builder.Entity<Movie>().Property(m => m.Description).IsRequired(false).HasMaxLength(2000);
            builder.Entity<Movie>().HasOne(mt => mt.MovieType).WithMany(m => m.Movies).HasForeignKey(m => m.MovieTypeId);
            
            builder.Entity<MovieUser>(entity =>
            {
                entity.ToTable("MovieUsers", tb => tb.HasTrigger("TR_MovieUsers_UpdateLikesCount"));
            });
            
            builder.Entity<WatchList>(entity =>
            {
                entity.ToTable("WatchLists", tb => tb.HasTrigger("TR_WatchLists_UpdateWatchListsCount"));
            });

            builder.Entity<WatchList>()
                .HasKey(wl => new { wl.MovieId, UserId = wl.ApplicationUserId });

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
