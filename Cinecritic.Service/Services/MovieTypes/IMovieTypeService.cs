using Cinecritic.Application.DTOs.MovieTypes;
using FluentResults;

namespace Cinecritic.Application.Services.MovieTypes
{
    public interface IMovieTypeService
    {
        Task<Result<IEnumerable<MovieTypeDto>>> GetMovieTypes();
    }
}