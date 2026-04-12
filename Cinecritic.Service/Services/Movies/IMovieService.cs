using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.Movies
{
    public interface IMovieService
    {
        Task<Result<Guid>> CreateMovieAsync(CreateMovieDto dto, Stream? stream, string? fileExtension);
        Task<Result<MovieWithReviewsDto>> GetMovieAsync(Guid movieId, Guid? userId, int reviewCount = 10);
        Task<Result<GetMoviesResultDto>> GetMoviesAsync(int pageSize, int pageCount);
        Task<Result<IEnumerable<FilmingLocation>>> GetNearestLocationsAsync(double latitude, double longitude, int nPoints = 5);
        Task<Result> UpdateMovieAsync(Guid id, CreateMovieDto dto, Stream? stream, string? fileExtension);
        Task<Result> DeleteMovieAsync(Guid id);
    }
}