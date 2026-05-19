using Cinecritic.Application.Repositories;
using Cinecritic.Infrastructure.Data;
using Cinecritic.Infrastructure.Options;
using Cinecritic.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Cinecritic.Infrastructure
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Account/Login";
            });

            services.Configure<EmailConfigurationOption>(configuration.GetSection("EMAIL"));
            
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieUserRepository, MovieUserRepository>();
            services.AddScoped<IWatchListRepository, WatchListRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            return services;
        }
    }
}
