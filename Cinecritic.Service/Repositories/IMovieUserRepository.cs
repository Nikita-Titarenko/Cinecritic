using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Domain.Models;

namespace Cinecritic.Application.Repositories
{
    public interface IMovieUserRepository : IRepository<MovieUser>
    {
        Task<int> CountLiked(Guid userId);
        Task<int> CountWatched(Guid userId);
        Task<IEnumerable<Movie>> GetLikedMoviesAsync(Guid userId, int pageSize, int pageCount);
        Task<MovieUser?> GetMovieUserAsync(Guid movieId, Guid userId);
        Task<IEnumerable<Movie>> GetWatchedMoviesAsync(Guid userId, int pageSize, int pageCount);
        Task<IEnumerable<Movie>> GetInWatchListMoviesAsync(Guid userId, int pageSize, int pageCount);
        Task<MovieStatisticsDto> GetMovieStatisticsAsync(Guid movieId);
    }
}