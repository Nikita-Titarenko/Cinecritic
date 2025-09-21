using Cinecritic.Application.DTOs.Reviews;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using Cinecritic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinecritic.Infrastructure.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MovieReviewDto>> GetMovieReviews(int movieId, int pageNumber, int pageSize)
        {
            return await _context.MovieUsers
                .Where(mu => mu.MovieId == movieId && mu.Review != null)
                .OrderByDescending(mu => mu.Review!.ReviewDateTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(mu => new
                {
                    mu.MovieId,
                    mu.UserId,
                    mu.IsLiked,
                    mu.Rate,
                    mu.Review
                })
                .Select(mu => new MovieReviewDto
                {
                    MovieId = movieId,
                    UserId = mu.UserId,
                    IsLiked = mu.IsLiked,
                    Rate = mu.Rate,
                    ReviewText = mu.Review!.ReviewText,
                    ReviewDate = DateOnly.FromDateTime(mu.Review.ReviewDateTime.Date),
                    DisplayName = _context.Users
                        .Where(u => u.Id == mu.Review.UserId)
                        .Select(u => u.DisplayName).First()
                })
                .ToListAsync();
        }
    }
}
