using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;
using MongoDB.Bson;

namespace Cinecritic.Application.Repositories;

public interface IMovieRepository : IRepository<Movie>
{
    Task<IEnumerable<Movie>> GetMoviesAsync(int pageSize, int pageCount);
    Task UpdateMovieStatsAsync(ObjectId movieId, MovieStatisticsDto stats);
    Task<MovieWithReviewsDto?> GetMovieWithReviewsAsync(ObjectId movieId, ObjectId? userId);
    Task<IEnumerable<FilmingLocation>> GetNearestFilmingLocationsAsync(GeoData myLocation, int nPoints);
}
