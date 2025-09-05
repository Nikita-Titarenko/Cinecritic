using Cinecritic.Application.DTOs;
using FluentResults;

namespace Cinecritic.Application.Services.Movies
{
    public interface IMovieService
    {
        Task<Result<int>> CreateMovieAsync(CreateMovieDto dto, Stream? stream, string? fileExtension);
        Task<Result<IEnumerable<MovieListItemDto>>> GetMoviesAsync(int pageSize, int pageCount);
    }
}