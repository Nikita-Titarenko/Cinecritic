using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using Cinecritic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinecritic.Infrastructure.Repositories
{
    public class MovieUserRepository : Repository<MovieUser>, IMovieUserRepository
    {
        public MovieUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<MovieUser?> GetMovieUserWithReview(int movieId, string userId)
        {
            return await _dbSet.Include(mu => mu.Review).FirstOrDefaultAsync(mu => mu.MovieId == movieId && mu.UserId == userId);
        }

        public async Task<MovieDto?> GetMovieAsync(int movieId, string userId)
        {
            var dto = await _context.Movies
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
                    .Select(mu => new
                    {
                        mu.IsLiked,
                        mu.Rate,
                        ReviewText = mu.Review != null ? mu.Review.ReviewText : null,
                        ReviewDate = mu.Review != null ? DateOnly.FromDateTime(mu.Review.ReviewDateTime.Date) : (DateOnly?)null
                    })
                    .FirstOrDefault(),
                    WatchList = m.WatchList
                    .Where(mu => mu.MovieId == movieId && mu.UserId == userId)
                    .FirstOrDefault(),
                    WatchedCount = m.MovieUsers
                        .Where(mu => mu.MovieId == movieId)
                        .Count(),
                    LikedCount = m.MovieUsers
                        .Where(mu => mu.MovieId == movieId && mu.IsLiked)
                        .Count(),
                    WatchListCount = m.WatchList
                        .Where(mu => mu.MovieId == movieId)
                        .Count(),
                    Rate = m.MovieUsers
                        .Where(mu => mu.MovieId == movieId)
                        .Average(mu => mu.Rate),
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
                        IsInWatchList = m.WatchList != null,
                        ReviewText = m.MovieUser != null ? m.MovieUser.ReviewText : null,
                        ReviewDate = m.MovieUser != null ? m.MovieUser.ReviewDate : null
                    },
                    WatchedCount = m.WatchedCount,
                    LikedCount = m.LikedCount,
                    WatchListCount = m.WatchListCount,
                    Rate = m.Rate ?? 0
                })
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                return null;
            }

            return dto;
        }

        public async Task<IEnumerable<MovieListItemDto>> GetWatchedMoviesAsync(string userId, int pageSize, int pageCount)
        {
            IEnumerable<MovieListItemDto> dto = await _dbSet
                .AsNoTracking()
                .Where(mu => mu.UserId == userId)
                .OrderByDescending(mu => mu.WatchedDateTime)
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

        public async Task<int> CountWatched(string userId)
        {
            return await _dbSet
                .Where(mu => mu.UserId == userId)
                .CountAsync();
        }

        public async Task<IEnumerable<MovieListItemDto>> GetLikedMoviesAsync(string userId, int pageSize, int pageCount)
        {
            IEnumerable<MovieListItemDto> dto = await _dbSet
                .AsNoTracking()
                .Where(mu => mu.UserId == userId && mu.IsLiked)
                .OrderByDescending(mu => mu.LikedDateTime)
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

        public async Task<int> CountLiked(string userId)
        {
            return await _dbSet
                .Where(mu => mu.UserId == userId && mu.IsLiked)
                .CountAsync();
        }
    }
}
