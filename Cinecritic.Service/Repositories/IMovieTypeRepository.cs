using Cinecritic.Domain.Models;

namespace Cinecritic.Application.Repositories
{
    public interface IMovieTypeRepository
    {
        Task<IEnumerable<MovieType>> GetMovieTypes();
    }
}