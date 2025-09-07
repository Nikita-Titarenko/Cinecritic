using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using Cinecritic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinecritic.Infrastructure.Repositories
{
    public class MovieTypeRepository : IMovieTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieTypeRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<MovieType>> GetMovieTypes()
        {
            return await _context.MovieTypes.ToListAsync();
        }
    }
}
