using Cinecritic.Application.DTOs.Movies;

namespace Cinecritic.Application.Repositories
{
    public interface IWatchListRepository
    {
        Task<int> Count(string userId);
        Task<IEnumerable<MovieListItemDto>> GetMoviesInWatchListAsync(string userId, int pageSize, int pageCount);
    }
}