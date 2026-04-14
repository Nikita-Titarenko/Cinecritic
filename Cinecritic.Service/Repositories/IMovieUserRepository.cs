using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;
using MongoDB.Bson;

namespace Cinecritic.Application.Repositories
{
    public interface IMovieUserRepository : IRepository<MovieUser>
    {
        Task<int> CountLiked(ObjectId userId);
        Task<int> CountWatched(ObjectId userId);
        Task<IEnumerable<Movie>> GetLikedMoviesAsync(ObjectId userId, int pageSize, int pageCount);
        Task<MovieUser?> GetMovieUserAsync(ObjectId movieId, ObjectId userId);
        Task<IEnumerable<Movie>> GetWatchedMoviesAsync(ObjectId userId, int pageSize, int pageCount);
        Task<IEnumerable<Movie>> GetInWatchListMoviesAsync(ObjectId userId, int pageSize, int pageCount);
        Task<MovieStatisticsDto> GetMovieStatisticsAsync(ObjectId movieId);
    }
}