using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Domain.Models;

namespace Cinecritic.Application.Repositories
{
    public interface IMovieUserRepository : IRepository<MovieUser>
    {
        Task<int> CountLiked(string userId);
        Task<int> CountWatched(string userId);
        Task<IEnumerable<MovieListItemDto>> GetLikedMoviesAsync(string userId, int pageSize, int pageCount);
        Task<MovieDto?> GetMovieAsync(int movieId, string userId);
        Task<MovieUser?> GetMovieUserWithReview(int movieId, string userId);
        Task<IEnumerable<MovieListItemDto>> GetWatchedMoviesAsync(string userId, int pageSize, int pageCount);
    }
}