using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using Cinecritic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinecritic.Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MovieListItemDto>> GetMoviesAsync(int pageSize, int pageCount)
        {
            IEnumerable<MovieListItemDto> dto = await _dbSet
                .AsNoTracking()
                .Skip((pageCount - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MovieListItemDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                }
               ).ToListAsync();

            return dto;
        }

        public async Task<MovieDto?> GetMovieAsync(int movieId, string userId)
        {
            var dto = await _dbSet
                .AsNoTracking()
                .Where(m => m.Id == movieId)
                .Select(m => new
                {
                    m.Id,
                    m.Title,
                    m.Description,
                    m.ReleaseDate,
                    MovieUser = m.MovieUsers
                    .Where(mu => mu.MovieId == movieId && mu.UserId == userId)
                    .FirstOrDefault(),
                    WatchList = m.WatchList
                    .Where(mu => mu.MovieId == movieId && mu.UserId == userId)
                    .FirstOrDefault()
                })
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    ReleaseDate = m.ReleaseDate,
                    MovieUserStatus = new MovieUserStatusDto
                    {
                        UserId = userId,
                        IsWatched = m.MovieUser != null,
                        IsLiked = m.MovieUser != null && m.MovieUser.IsLiked,
                        Rate = m.MovieUser != null ? m.MovieUser.Rate : null,
                        IsInWatchList = m.WatchList != null
                    }
                })
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                return null;
            }

            return dto;
        }
    }
}
