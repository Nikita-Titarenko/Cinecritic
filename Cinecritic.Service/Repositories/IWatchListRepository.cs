using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;

namespace Cinecritic.Application.Repositories
{
    public interface IWatchListRepository : IRepository<WatchList>
    {
        Task<int> Count(int userId);
        Task<IEnumerable<MovieListItemDto>> GetMoviesInWatchListAsync(int userId, int pageSize, int pageCount);
    }
}