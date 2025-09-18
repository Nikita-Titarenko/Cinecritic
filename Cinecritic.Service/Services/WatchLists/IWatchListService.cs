using Cinecritic.Application.DTOs.MovieUsers;
using FluentResults;

namespace Cinecritic.Application.Services.WatchLists
{
    public interface IWatchListService
    {
        Task<Result<MovieUserStatusDto>> ToggleWatchListMovieAsync(int movieId, string userId);
    }
}