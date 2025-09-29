using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using Cinecritic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinecritic.Infrastructure.Repositories
{
    public class WatchListRepository : Repository<WatchList>, IWatchListRepository
    {
        public WatchListRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MovieListItemDto>> GetMoviesInWatchListAsync(string userId, int pageSize, int pageCount)
        {
            IEnumerable<MovieListItemDto> dto = await _dbSet
                .AsNoTracking()
                .Where(mu => mu.UserId == userId)
                .OrderByDescending(mu => mu.InWatchListDateTime)
                .Skip((pageCount - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MovieListItemDto
                {
                    Id = m.MovieId,
                    Title = m.Movie.Title,
                    ReleaseDate = m.Movie.ReleaseDate,
                }
               ).ToListAsync();

            return dto;
        }

        public async Task<int> Count(string userId)
        {
            return await _dbSet
                .Where(mu => mu.UserId == userId)
                .CountAsync();
        }
    }
}
