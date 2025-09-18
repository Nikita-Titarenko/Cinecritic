using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.DTOs.Reviews;
using FluentResults;

namespace Cinecritic.Application.Services.Reviews
{
    public interface IReviewService
    {
        Task<Result<MovieUserStatusDto>> CreateMovieReviewAsync(UpsertMovieReviewDto dto);
        Task<Result<IEnumerable<MovieReviewDto>>> GetMovieReviews(int movieId, int pageNumber, int pageSize);
        Task<Result<MovieUserStatusDto>> UpdateMovieReviewAsync(UpsertMovieReviewDto dto);
    }
}