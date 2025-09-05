using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using Cinecritic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinecritic.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie.Id;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync(int pageSize, int pageCount)
        {
            return await _context.Movies
                .AsNoTracking()
                .Skip((pageCount - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new Movie {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate
                }
               ).ToListAsync();
        }
    }
}
