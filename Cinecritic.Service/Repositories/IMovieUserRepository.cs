using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Domain.Models;

namespace Cinecritic.Application.Repositories
{
    public interface IMovieUserRepository : IRepository<MovieUser>
    {
        Task<MovieUser?> GetMovieUserWithReview(int movieId, string userId);
    }
}