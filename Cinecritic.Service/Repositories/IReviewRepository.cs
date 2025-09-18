using Cinecritic.Application.DTOs.MovieUsers;

namespace Cinecritic.Application.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<MovieReviewDto>> GetMovieReviews(int movieId, int pageNumber, int pageSize);
    }
}