using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Emails;
using Cinecritic.Application.Services.Files;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieTypes;
using Cinecritic.Application.Services.MovieUsers;
using Cinecritic.Application.Services.Reviews;
using Cinecritic.Application.Services.Users;
using Cinecritic.Application.Services.WatchLists;
using Cinecritic.Infrastructure.Data;
using Cinecritic.Infrastructure.Repositories;
using Cinecritic.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cinecritic.Infrastructure
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IMovieUserService, MovieUserService>();
            services.AddScoped<IWatchListService, WatchListService>();
            services.AddScoped<IMovieTypeService, MovieTypeService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<IEmailSender, SmtpEmailSender>();
            services.AddSingleton<IEmailSender<ApplicationUser>, ApplicationUserEmailSender>();
            services.AddAutoMapper((a) => { }, AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
