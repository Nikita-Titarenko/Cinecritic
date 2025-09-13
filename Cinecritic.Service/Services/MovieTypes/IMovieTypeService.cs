using Cinecritic.Application.DTOs.Movies;
using FluentResults;

namespace Cinecritic.Application.Services.MovieTypes
{
    public interface IMovieTypeService
    {
        Task<Result<IEnumerable<MovieTypeDto>>> GetMovieTypes();
    }
}