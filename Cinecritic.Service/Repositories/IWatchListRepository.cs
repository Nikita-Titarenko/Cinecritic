// using Cinecritic.Application.DTOs.Movies;
// using Cinecritic.Domain.Models;
//
// namespace Cinecritic.Application.Repositories
// {
//     public interface IWatchListRepository : IRepository<WatchList>
//     {
//         Task<long> Count(Guid userId);
//         Task<IEnumerable<Movie>> GetMoviesInWatchListAsync(Guid userId, int pageSize, int pageCount);
//         Task DeleteByMovieAndUserAsync(Guid movieId, Guid userId);
//     }
// }