using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Email;
using Cinecritic.Application.Services.Files;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieType;
using Cinecritic.Application.Services.Users;
using Cinecritic.Infrastructure.Data;
using Cinecritic.Infrastructure.Repositories;
using Cinecritic.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Vulyk.Infrastructure.Services.User;

namespace Cinecritic.Infrastructure
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieTypeService, MovieTypeService>();
            services.AddScoped<IMovieTypeRepository, MovieTypeRepository>();
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
