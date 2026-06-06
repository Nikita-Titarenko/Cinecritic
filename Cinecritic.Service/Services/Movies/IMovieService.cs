using Cinecritic.Application.DTOs.Movies;
using FluentResults;

namespace Cinecritic.Application.Services.Movies
{
    public interface IMovieService
    {
        Task<Result<int>> CreateMovieAsync(CreateMovieDto dto, Stream? stream, string? fileExtension);
        Task<Result<MovieDto>> GetMovieAsync(int movieId, int userId, int reviewCount = 10);
        Task<Result<GetMoviesResultDto>> GetMoviesAsync(int pageSize, int pageCount);
        Task<Result<int>> UpdateMovieAsync(int movieId, CreateMovieDto dto, Stream? stream, string? fileExtension);
        Task<Result<int>> DeleteMovieAsync(int movieId);

        Task<IEnumerable<TopMovieQueryResult>> GetTopMoviesByTypeAsync(
            int movieTypeId,
            decimal minRating,
            int pageNumber,
            int pageSize,
            string? userId);
    }
}