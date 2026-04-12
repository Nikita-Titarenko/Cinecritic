using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinecritic.Application.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetMoviesAsync(int pageSize, int pageCount);
        Task UpdateMovieStatsAsync(Guid movieId, MovieStatisticsDto stats);
        Task<MovieWithReviewsDto?> GetMovieWithReviewsAsync(Guid movieId, Guid? userId);
        Task<IEnumerable<FilmingLocation>> GetNearestFilmingLocationsAsync(GeoData myLocation, int nPoints);
    }
}
