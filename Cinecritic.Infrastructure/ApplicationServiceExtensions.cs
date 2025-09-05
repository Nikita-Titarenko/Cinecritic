using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Files;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Infrastructure.Data;
using Cinecritic.Infrastructure.Repositories;
using Cinecritic.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cinecritic.Infrastructure
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IFileService, FileService>();
            services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
            services.AddAutoMapper((a) => { }, AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
