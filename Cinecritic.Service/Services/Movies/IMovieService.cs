using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;
using FluentResults;
using MongoDB.Bson;

namespace Cinecritic.Application.Services.Movies
{
    public interface IMovieService
    {
        Task<Result<ObjectId>> CreateMovieAsync(CreateMovieDto dto, Stream? stream, string? fileExtension);
        Task<Result<MovieWithReviewsDto>> GetMovieAsync(ObjectId movieId, ObjectId? userId, int reviewCount = 10);
        Task<Result<GetMoviesResultDto>> GetMoviesAsync(int pageSize, int pageCount);
        Task<Result<IEnumerable<FilmingLocation>>> GetNearestLocationsAsync(double latitude, double longitude, int nPoints = 5);
        Task<Result> UpdateMovieAsync(ObjectId id, CreateMovieDto dto, Stream? stream, string? fileExtension);
        Task<Result> DeleteMovieAsync(ObjectId id);
    }
}