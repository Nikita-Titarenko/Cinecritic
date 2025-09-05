using Cinecritic.Domain.Models;

namespace Cinecritic.Application.Repositories
{
    public interface IMovieRepository
    {
        Task<int> CreateMovieAsync(Movie movie);
        Task<IEnumerable<Movie>> GetMoviesAsync(int pageSize, int pageCount);
    }
}