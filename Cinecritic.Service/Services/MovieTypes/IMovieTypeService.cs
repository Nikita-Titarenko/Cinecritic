using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.MovieTypes;

public interface IMovieTypeService
{
    Task<Result<IEnumerable<MovieType>>> GetMovieTypes();
}