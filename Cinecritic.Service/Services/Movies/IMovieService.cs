using Cinecritic.Application.DTOs.Movies;
using FluentResults;

namespace Cinecritic.Application.Services.Movies
{
    public interface IMovieService
    {
        Task<Result<int>> CreateMovieAsync(CreateMovieDto dto, Stream? stream, string? fileExtension);
        Task<Result<MovieDto>> GetMovieAsync(int movieId, string userId, int reviewCount = 10);
        Task<Result<IEnumerable<MovieListItemDto>>> GetMoviesAsync(int pageSize, int pageCount);
    }
}