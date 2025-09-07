using Cinecritic.Application.DTOs.Movies;
using FluentResults;

namespace Cinecritic.Application.Services.MovieType
{
    public interface IMovieTypeService
    {
        Task<Result<IEnumerable<MovieTypeDto>>> GetMovieTypes();
    }
}