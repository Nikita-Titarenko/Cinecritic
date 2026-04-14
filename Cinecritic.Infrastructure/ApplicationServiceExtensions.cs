using Cinecritic.Application.Services.Files;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieTypes;
using Cinecritic.Application.Services.MovieUsers;
using Cinecritic.Application.Services.Users;
using Cinecritic.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cinecritic.Infrastructure;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IMovieUserService, MovieUserService>();
        services.AddScoped<IMovieTypeService, MovieTypeService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFileService, FileService>();

        services.AddAutoMapper((a) => { }, AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}
