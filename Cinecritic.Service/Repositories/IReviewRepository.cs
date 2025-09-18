using Cinecritic.Application.DTOs.Reviews;

namespace Cinecritic.Application.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<MovieReviewDto>> GetMovieReviews(int movieId, int pageNumber, int pageSize);
    }
}