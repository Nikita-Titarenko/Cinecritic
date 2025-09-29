using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using FluentResults;

namespace Cinecritic.Application.Services.MovieUsers
{
    public interface IMovieUserService
    {
        Task DeleteFromWatchListAsync(int movieId, string userId);
        Task<Result<GetMoviesResultDto>> GetLikedMoviesAsync(string userId, int pageSize, int pageCount);
        Task<Result<GetMoviesResultDto>> GetWatchedMoviesAsync(string userId, int pageSize, int pageCount);
        Task<Result<MovieUserStatusDto>> RateMovieAsync(RateMovieDto rateMovieDto);
        Task<Result<MovieUserStatusDto>> ToggleLikeMovieAsync(int movieId, string userId);
        Task<Result<MovieUserStatusDto>> ToggleWatchMovieAsync(int movieId, string userId);
    }
}