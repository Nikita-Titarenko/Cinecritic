using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieTypes;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using Cinecritic.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Cinecritic.Infrastructure.Repositories
{
    public class MovieUserRepository : Repository<MovieUser>, IMovieUserRepository
    {
        public MovieUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<MovieUser?> GetMovieUserAsync(int movieId, int userId)
        {
            return _dbSet.FirstOrDefaultAsync(mu => mu.MovieId == movieId && mu.ApplicationUserId == userId);
        }

        public async Task<MovieUser?> GetMovieUserWithReviewAsync(int movieId, int userId)
        {
            return await _dbSet.Include(mu => mu.Review).FirstOrDefaultAsync(mu => mu.MovieId == movieId && mu.ApplicationUserId == userId);
        }

        public async Task<MovieDto?> GetMovieAsync(int movieId, int userId)
        {
            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.Id == movieId)
                .Select(m => new
                {
                    m.Id,
                    m.Title,
                    m.Description,
                    m.ReleaseDate,
                    m.MovieType,
                    MovieUser = m.MovieUsers
                        .Where(mu => mu.MovieId == movieId && mu.ApplicationUserId == userId)
                        .Select(mu => new
                        {
                            mu.IsLiked,
                            mu.Rate,
                            ReviewText = mu.Review != null ? mu.Review.ReviewText : null,
                            ReviewDate = mu.Review != null
                                ? DateOnly.FromDateTime(mu.Review.ReviewDateTime.Date)
                                : (DateOnly?)null
                        })
                        .FirstOrDefault(),
                    WatchList = m.WatchList
                        .FirstOrDefault(mu => mu.MovieId == movieId && mu.ApplicationUserId == userId),
                    WatchedCount = m.WatchCount,
                    LikedCount = m.LikesCount,
                    WatchListCount = m.WatchListCount,
                    Rate = _context.GetMovieAverageRating(movieId),
                })
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    ReleaseDate = m.ReleaseDate,
                    MovieType = new MovieTypeDto
                    {
                        Id = m.MovieType.Id,
                        MovieTypeName = m.MovieType.MovieTypeName
                    },
                    MovieUserStatus = new MovieUserStatusDto
                    {
                        ApplicationUserId = userId,
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
                    Rate = m.Rate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MovieListItemDto>> GetWatchedMoviesAsync(int userId, int pageSize, int pageCount)
        {
            IEnumerable<MovieListItemDto> dto = await _dbSet
                .AsNoTracking()
                .Where(mu => mu.ApplicationUserId == userId)
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

        public async Task<int> CountWatched(int userId)
        {
            return await _dbSet
                .Where(mu => mu.ApplicationUserId == userId)
                .CountAsync();
        }

        public async Task<IEnumerable<MovieListItemDto>> GetLikedMoviesAsync(int userId, int pageSize, int pageCount)
        {
            IEnumerable<MovieListItemDto> dto = await _dbSet
                .AsNoTracking()
                .Where(mu => mu.ApplicationUserId == userId && mu.IsLiked)
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

        public async Task<int> CountLiked(int userId)
        {
            return await _dbSet
                .Where(mu => mu.ApplicationUserId == userId && mu.IsLiked)
                .CountAsync();
        }

        public async Task UpsertMovieUserLikeAndRatingAsync(int movieId, string userId, bool isLiked, int? rate)
        {
            var movieIdParam = new SqlParameter("@MovieId", movieId);
            var userIdParam = new SqlParameter("@ApplicationUserId", userId);
            var isLikedParam = new SqlParameter("@IsLiked", isLiked);
            var rateParam = new SqlParameter("@Rate", (object)rate ?? DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.UpsertMovieUserLikeAndRating @MovieId, @ApplicationUserId, @IsLiked, @Rate",
                movieIdParam, userIdParam, isLikedParam, rateParam);
        }
    }
}
