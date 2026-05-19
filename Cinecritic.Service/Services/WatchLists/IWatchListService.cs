using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using FluentResults;

namespace Cinecritic.Application.Services.WatchLists
{
    public interface IWatchListService
    {
        Task<Result<GetMoviesResultDto>> GetMoviesInWatchListAsync(int userId, int pageSize, int pageCount);
        Task<Result<MovieUserStatusDto>> ToggleWatchListMovieAsync(int movieId, int userId);
    }
}