using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
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
                .OrderByDescending(m => m.ReleaseDate)
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
    }
}
