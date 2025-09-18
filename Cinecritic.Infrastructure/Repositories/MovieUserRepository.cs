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
    }
}
