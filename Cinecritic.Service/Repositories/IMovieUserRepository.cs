using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Domain.Models;

namespace Cinecritic.Application.Repositories
{
    public interface IMovieUserRepository : IRepository<MovieUser>
    {
        Task<int> CountLiked(int userId);
        Task<int> CountWatched(int userId);
        Task<IEnumerable<MovieListItemDto>> GetLikedMoviesAsync(int userId, int pageSize, int pageCount);
        Task<MovieDto?> GetMovieAsync(int movieId, int userId);
        Task<MovieUser?> GetMovieUserWithReviewAsync(int movieId, int userId);
        Task<IEnumerable<MovieListItemDto>> GetWatchedMoviesAsync(int userId, int pageSize, int pageCount);
        Task UpsertMovieUserLikeAndRatingAsync(int movieId, string userId, bool isLiked, int? rate);
    }
}