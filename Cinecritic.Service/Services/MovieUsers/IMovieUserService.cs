using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using FluentResults;

namespace Cinecritic.Application.Services.MovieUsers
{
    public interface IMovieUserService
    {
        Task DeleteFromWatchListAsync(int movieId, int userId);
        Task<Result<GetMoviesResultDto>> GetLikedMoviesAsync(int userId, int pageSize, int pageCount);
        Task<Result<GetMoviesResultDto>> GetWatchedMoviesAsync(int userId, int pageSize, int pageCount);
        Task<Result<MovieUserStatusDto>> RateMovieAsync(RateMovieDto rateMovieDto);
        Task<Result<MovieUserStatusDto>> ToggleLikeMovieAsync(int movieId, int userId);
        Task<Result<MovieUserStatusDto>> ToggleWatchMovieAsync(int movieId, int userId);
    }
}