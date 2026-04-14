using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;
using FluentResults;
using MongoDB.Bson;

namespace Cinecritic.Application.Services.MovieUsers
{
    public interface IMovieUserService
    {
        Task<Result<GetMoviesResultDto>> GetLikedMoviesAsync(ObjectId userId, int pageSize, int pageCount);
        Task<Result<GetMoviesResultDto>> GetWatchedMoviesAsync(ObjectId userId, int pageSize, int pageCount);
        Task<Result<MovieUser>> RateMovieAsync(ObjectId movieId, ObjectId userId, int rating);
        Task<Result<MovieUser>> ToggleLikeMovieAsync(ObjectId movieId, ObjectId userId);
        Task<Result<MovieUser>> ToggleWatchMovieAsync(ObjectId movieId, ObjectId userId);
        Task<Result<MovieUser>> ToggleIsInWatchListAsync(ObjectId movieId, ObjectId userId);
        Task<Result<MovieUser>> CreateOrUpdateReviewAsync(ObjectId movieId, ObjectId userId, string reviewText);
        Task<Result<GetMoviesResultDto>> GetInWatchListMoviesAsync(ObjectId userId, int pageSize, int pageCount);
    }
}