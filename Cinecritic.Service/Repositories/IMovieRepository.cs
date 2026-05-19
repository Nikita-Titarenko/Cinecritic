using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinecritic.Application.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<IEnumerable<MovieListItemDto>> GetMoviesAsync(int pageSize, int pageCount);

        Task<IEnumerable<TopMovieQueryResult>> GetTopMoviesByTypeAsync(
            int movieTypeId, 
            decimal minRating, 
            int pageNumber, 
            int pageSize, 
            string? userId);
    }
}
