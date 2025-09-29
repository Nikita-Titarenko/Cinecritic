using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using FluentResults;

namespace Cinecritic.Application.Services.WatchLists
{
    public interface IWatchListService
    {
        Task<Result<GetMoviesResultDto>> GetMoviesInWatchListAsync(string userId, int pageSize, int pageCount);
        Task<Result<MovieUserStatusDto>> ToggleWatchListMovieAsync(int movieId, string userId);
    }
}