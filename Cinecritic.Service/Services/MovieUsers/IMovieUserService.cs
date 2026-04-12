using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.MovieUsers
{
    public interface IMovieUserService
    {
        Task<Result<GetMoviesResultDto>> GetLikedMoviesAsync(Guid userId, int pageSize, int pageCount);
        Task<Result<GetMoviesResultDto>> GetWatchedMoviesAsync(Guid userId, int pageSize, int pageCount);
        Task<Result<MovieUser>> RateMovieAsync(Guid movieId, Guid userId, int rating);
        Task<Result<MovieUser>> ToggleLikeMovieAsync(Guid movieId, Guid userId);
        Task<Result<MovieUser>> ToggleWatchMovieAsync(Guid movieId, Guid userId);
        Task<Result<MovieUser>> ToggleIsInWatchListAsync(Guid movieId, Guid userId);
        Task<Result<MovieUser>> CreateOrUpdateReviewAsync(Guid movieId, Guid userId, string reviewText);
        Task<Result<GetMoviesResultDto>> GetInWatchListMoviesAsync(Guid userId, int pageSize, int pageCount);
    }
}